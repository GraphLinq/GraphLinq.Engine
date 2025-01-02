using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NodeBlock.Engine.Encoding;
using NodeBlock.Engine.Enums;
using NodeBlock.Engine.Storage.Redis;
using System.Linq;
using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using NodeBlock.Engine.Storage.MariaDB;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;
using NodeBlock.Engine.Storage.MariaDB.Entities;
using NodeBlock.Engine.Storage;

namespace NodeBlock.Engine
{
    public class GraphContextWrapper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public GraphStateEnum currentGraphState { get; set; }
        public BlockGraph graph { get; set; }
        public Thread context { get; set; }
        
        public int walletIdentifier { get; set; }

        public DateTime LoadedAt { get; set; }

        public DateTime? StoppedAt { get; set; }

        public decimal CurrentBulkCost { get; set; }

        public object threadMutex = new object();

        public GraphContextWrapper(BlockGraph _graph, int _walletIdentifier, 
            GraphStateEnum _initialState)
        {
            this.graph = _graph;
            this.walletIdentifier = _walletIdentifier;
            this.currentGraphState = _initialState;

            this.context = Thread.CurrentThread;
            this.StoppedAt = null;
        }

        public void InitContext(bool engineInit = false)
        {
            int totalThreads = Process.GetCurrentProcess().Threads.Count;

            logger.Info(string.Format("New graph loaded of {0} bytes (hash: {1}, currentState: {2}, {3} threads running)",
                     graph.RawGraphData.Length, graph.UniqueHash, currentGraphState.ToString(), Convert.ToString(totalThreads)));

            if (engineInit || currentGraphState == GraphStateEnum.STARTING)
            {
                try
                {
                    this.LoadedAt = DateTime.UtcNow;
                    graph.Start(this);
                }
                finally
                {
                    if (graph.IsRunning)
                    {
                        graph.AppendLog("warn", string.Format("Graph hash {0} started its execution.", graph.UniqueHash));
                        logger.Info("Graph hash {0} started its execution.", graph.UniqueHash);
                    }
                }
            }
        }

        public void AddCycleCost(decimal cost)
        {
            lock (this.threadMutex)
            {
                CurrentBulkCost += cost;
            }
        }

        public void RemoveCycleCost(decimal cost)
        {
            lock (this.threadMutex)
            {
                CurrentBulkCost -= cost;
                if (CurrentBulkCost < 0) { CurrentBulkCost = 0; }
            }
        }
    }

    
    public static class GraphsContainer
    {
        public static long StartAt = 0;

        private static bool running = true;
        private static readonly Dictionary<string, GraphContextWrapper> _graphs = new Dictionary<string, GraphContextWrapper>();
        private static readonly object mutex = new object();
        private static ServiceProvider services;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static GraphsContainer()
        {
            StartAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            services = new ServiceCollection()
            .AddScoped(provider => provider.GetService<MariaDBStorage>())
            .AddDbContextPool<MariaDBStorage>(options =>
            {
                options.UseMySQL(
                    Environment.GetEnvironmentVariable("mariadb_uri"));
            })
            .BuildServiceProvider();

        }

       public static ServiceProvider GetServiceProvider()
       {
            return services;
       }

        public static bool InitActiveGraphs()
        {
            var graphStorages = StorageManager.GetStorage().GetGraphStorages();
            logger.Info("Starting back the context of {0} graph(s) from last execution", graphStorages.Count);

            graphStorages.ForEach(x =>
            {
                try
                {
                    var decompressedRaw = GraphCompression.DecompressGraphData(x.CompressedBytes);
                    var loadedGraph = BlockGraph.LoadGraph(decompressedRaw, x.StoredHash, x.CompressedBytes);
                    if (loadedGraph != null)
                        AddNewGraph(loadedGraph, x.WalletIdentifierOwner, x.StateGraph, true);
                }
                catch (Exception error)
                {
                    logger.Error(error);
                }
            });

            return true;
        }

        public static void InitConsumingGraphCosts()
        {
            new Task(() =>
            {
                while (running)
                {
                    List<GraphContextWrapper> runningGraphs;
                    lock (mutex)
                    {
                        runningGraphs = _graphs.Where(x => x.Value.currentGraphState == GraphStateEnum.STARTED).Select(x => x.Value).ToList();
                    }

                    runningGraphs.ForEach(async (x) =>
                    {
                        try
                        {
                            var cost = x.CurrentBulkCost;
                            if (cost > 0)
                            {
                                decimal decimalAmount = cost / decimal.Parse(Environment.GetEnvironmentVariable("factor_decimal"));
                                string precision = decimalAmount.ToString("N8");
                                decimalAmount = decimal.Parse(precision);

                                using (var scope = services.CreateScope())
                                {
                                    var context = scope.ServiceProvider.GetService<MariaDBStorage>();
                                    Wallet wallet = await context.FetchWalletById(x.walletIdentifier);
                                    Graph graph = await context.FetchGraphByHash(x.graph.UniqueHash);
                                    if (wallet != null)
                                        wallet.due_balance += decimalAmount;
                                    if (graph != null)
                                    {
                                        graph.last_execution_cost += decimalAmount;
                                        graph.total_cost += decimalAmount;
                                    }
                                    await context.SaveChangesAsync();
                                }

                                x.graph.AppendLog("warn", string.Format("Cost for {0} last cycle was {1} GLQ", x.graph.UniqueHash, decimalAmount));
                                logger.Info("Cost for {0} was {1} GLQ", x.graph.UniqueHash, decimalAmount);
                                x.RemoveCycleCost(cost);
                            }
                        }
                        catch (Exception error)
                        {
                            logger.Error(error);
                        }
                    });
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            }).Start();
        }

        public static void StopGraphByHash(string hash)
        {
            lock (mutex)
            {
                if (_graphs.ContainsKey(hash))
                {
                    _graphs[hash].StoppedAt = DateTime.UtcNow;
                    UpdateStorageStateGraph(_graphs[hash], GraphStateEnum.STOPPED);
                }
            }
        }

        public static void UpdateAliveStorage()
        {
            var lists = _graphs.Where(x => !x.Value.graph.Debug && x.Value.currentGraphState == GraphStateEnum.STARTED ||
                x.Value.currentGraphState == GraphStateEnum.RESTARTING).ToList().Select(x => x.Value.graph).ToList();
            StorageManager.GetStorage().SaveListActiveGraphs(lists);
        }

        public static void UpdateStorageStateGraph(GraphContextWrapper context, GraphStateEnum newState)
        {
            try
            {
                context.currentGraphState = newState;
                var graphStorage = StorageManager.GetStorage().SetGraphStorage(context.graph,
                    context.walletIdentifier, newState);
                if (context.graph != null && context.graph.IsRunning &&
                    newState == GraphStateEnum.STOPPED)
                    context.graph.Stop();

                UpdateAliveStorage();
            }
            catch (Exception error)
            {
                logger.Error(error);
            }
        }

        public static GraphContextWrapper GetRunningGraphByHash(string hash)
        {
            lock (mutex)
            {
                if (_graphs.ContainsKey(hash))
                {
                    return _graphs[hash];
                }
            }
            return null;
        }

        public static GraphContextWrapper GetRunningGraphByName(string name)
        {
            lock (mutex)
            {
                return _graphs.ToList().FirstOrDefault(x => x.Value.graph.Name == name).Value;
            }
            return null;
        }

        public static GraphContextWrapper GetWalletDebugGraph(int walletId)
        {
            lock (mutex)
            {
                var graph = _graphs.FirstOrDefault(x => x.Value.walletIdentifier == walletId && x.Value.graph.Debug);
                return graph.Value;
            }
        }

        public static void UpdateGraphInStorage(GraphContextWrapper graphContext, bool engineInit)
        {
            try
            {
                var graphStorage = StorageManager.GetStorage().SetGraphStorage(graphContext.graph,
                    graphContext.walletIdentifier, graphContext.currentGraphState);

                // to avoid Redis update on each started graph at engine init
                if (!engineInit)
                    UpdateAliveStorage();
            }
            catch (Exception error)
            {
                logger.Error(error);
            }
        }

        public static void RestartLoadedGraph(BlockGraph newGraph,
            int walletIdentifier, GraphStateEnum initialState)
        {
            try
            {
                GraphContextWrapper currentlyRunning = _graphs[newGraph.UniqueHash];
                if (currentlyRunning == null)
                    throw new Exception("Invalid graph loaded in memory pool.");
                UpdateStorageStateGraph(currentlyRunning, GraphStateEnum.RESTARTING);
                logger.Info("Reloading Graph hash {0}", newGraph.UniqueHash);
                if (currentlyRunning.graph != null && currentlyRunning.graph.IsRunning)
                    currentlyRunning.graph.Stop();
            }
            catch (Exception error)
            {
                logger.Error(error);
            }
            finally
            {
                Task.Run(() =>
                {
                    // wait for the end of current execution
                    while (_graphs[newGraph.UniqueHash].graph.IsRunning) { Thread.Sleep(1000); };

                    newGraph.AppendLog("warn", string.Format("Graph hash {0} stopped successfully, restarting...", newGraph.UniqueHash));
                    logger.Info("Graph hash {0} stopped successfully, restarting...", newGraph.UniqueHash);
                    _graphs.Remove(newGraph.UniqueHash);
                    AddNewGraph(newGraph, walletIdentifier, GraphStateEnum.STARTING);
                });
                
            }
        }

        public static bool AddNewGraph(BlockGraph graph,
            int walletIdentifier, GraphStateEnum initialState = GraphStateEnum.STOPPED, bool engineInit = false)
        {
            try
            {
                logger.Info("Starting graph " + graph.UniqueHash + " ...");
                // lock once to check if the graph is already loaded
                lock (mutex)
                {
                    if (_graphs.ContainsKey(graph.UniqueHash))
                    {
                            RestartLoadedGraph(graph, walletIdentifier, initialState);
                        return true;
                    }
                }

                new Thread(new ThreadStart(() =>
                {
                    // init the thread and init his context
                    lock (mutex)
                    {
                        if (_graphs.ContainsKey(graph.UniqueHash))
                            return;
                        //create and save the graph in the hash map
                        var wrapper = new GraphContextWrapper(graph, walletIdentifier, initialState);
                        _graphs.Add(graph.UniqueHash, wrapper);

                        // update graph context in database
                        UpdateGraphInStorage(wrapper, engineInit);

                        // start the graph execution context
                        wrapper.InitContext(engineInit);
                    }
                })).Start();

                return true;
            }
            catch (Exception error)
            {
                logger.Error(error);
                return false;
            }
        }

        public static Dictionary<string, GraphContextWrapper> GetGraphs()
        {
            return _graphs;
        }
    }
}
