﻿using Newtonsoft.Json;
using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Encoding;
using NodeBlock.Engine.Generics;
using NodeBlock.Engine.Interop;
using NodeBlock.Engine.Interop.Plugin;
using NodeBlock.Engine.Nodes;
using NodeBlock.Engine.Nodes.API;
using NodeBlock.Engine.Nodes.Encoding;
using NodeBlock.Engine.Nodes.Functions;
using NodeBlock.Engine.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace NodeBlock.Engine
{
    public class BlockGraph
    {
        private const uint MAX_FAILED_CYCLE = 20;

        public string Name { get; }
        public Dictionary<string, Node> Nodes { get; set; }

        public string RawGraphData { get; set; }

        public string CompressedRaw { get; set; }

        public string UniqueHash { get; set; }

        public GraphContextWrapper currentContext = null;

        public Dictionary<string, object> MemoryVariables = new Dictionary<string, object>();
        public GraphExecutionCycle currentCycle;
        public StackLimitList<GraphExecutionCycle> PreviousCycles = new StackLimitList<GraphExecutionCycle>(20);

        public bool IsRunning = false;
        public bool Debug = false;
        public int CycleCountSinceStart = 0;
        public long StartedAt = 0;
        public DateTime? RotateLastUpdate;

        // Events
        public static event EventHandler<BlockGraph> OnNewGraphLoaded;
        public event EventHandler OnGraphStarted;
        public event EventHandler OnGraphStopped;
        public event EventHandler OnNewCycle;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Dictionary<string, Task> queueTaskCycleThreads = new Dictionary<string, Task>();
        private Dictionary<string, ConcurrentQueue<GraphExecutionCycle>> pendingCyclesQueues = new Dictionary<string, ConcurrentQueue<GraphExecutionCycle>>();
        private CancellationTokenSource cancelCycleToken;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public BlockGraph(string name = "", Node entryPoint = null, bool createEntryPoint = true)
        {
            GraphManager.InitGraphEngine();
            PluginManager.LoadPlugins();
            this.Name = name;
            this.Nodes = new Dictionary<string, Node>();
            if(entryPoint == null)
            {
                if(createEntryPoint)
                {
                    // Create a default entry point
                    this.AddNode(new EntryPointNode(string.Empty, this));
                }
            }
            else
            {
                // Insert the given entry point
                this.AddNode(entryPoint);
            }
            if (OnNewGraphLoaded != null) OnNewGraphLoaded(this, this);
        }

        public void AddCycleToQueue(string queue, GraphExecutionCycle executionCycle)
        {
            if(!this.pendingCyclesQueues.ContainsKey(queue))
            {
                this.pendingCyclesQueues.Add(queue, new ConcurrentQueue<GraphExecutionCycle>());
                this.runQueueTask(queue);
            }
            this.pendingCyclesQueues[queue].Enqueue(executionCycle);
        }

        private void runQueueTask(string queueName)
        {
            this.cancelCycleToken = new CancellationTokenSource();
            var task = new Task(async () =>
            {
                int failedCycleCount = 0;
                while (!this.cancelCycleToken.IsCancellationRequested)
                {
                    if (this.pendingCyclesQueues[queueName].Count <= 0)
                    {
                        await Task.Delay(150);
                    }
                    GraphExecutionCycle pendingCycle = null;
                    if (this.cancelCycleToken.IsCancellationRequested) return;
                    while (this.pendingCyclesQueues[queueName].TryDequeue(out pendingCycle))
                    {
                        try
                        {
                            await semaphoreSlim.WaitAsync();
                            if (!this.IsRunning) return;
                            currentCycle = pendingCycle;
                            if (this.cancelCycleToken.IsCancellationRequested) return;

                            // Add the cycle to the previous cycle list
                            this.PreviousCycles.Push(pendingCycle);

                            Task cycleTask = new Task(() =>
                            {
                                pendingCycle.Execute();
                            });
                            try
                            {
                                Task timeoutTask = Task.Delay(pendingCycle.GetCycleMaxExecutionTime());
                                cycleTask.Start();
                                this.CycleCountSinceStart++;
                                var taskResult = await Task.WhenAny(cycleTask, timeoutTask);

                                if (timeoutTask == taskResult)
                                {
                                    this.AppendLog("error", string.Format("Timeout occured on last cycle from graph hash: {0}", this.UniqueHash));
                                    logger.Error("Timeout exceeded for the cycle, skipping ..");
                                    cycleTask.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex, "Error when executing the cycle");
                                failedCycleCount++;
                            }
                        }
                        catch (Exception ex2)
                        {
                            logger.Error(ex2, "Error when executing the cycle");
                            failedCycleCount++;
                        }
                        finally
                        {
                            currentCycle = null;
                            semaphoreSlim.Release();
                            //if(failedCycleCount >= MAX_FAILED_CYCLE)
                            //{
                            //    failedCycleCount = 0;
                            //    this.Stop(false);
                            //}
                        }
                    }
                }
                return;
            });
            queueTaskCycleThreads.Add(queueName, task);
            task.Start();
        }

        public bool Stop(bool force = false)
        {
            if (!this.IsRunning && !force) return true;
            this.Nodes.ToList().FindAll(x => x.Value.IsEventNode).ForEach(x =>
            {
                try
                {
                    x.Value.OnStop();
                }
                catch(Exception ex)
                {
                    this.AppendLog("error", "Can't release the connector " + x.Value.FriendlyName + " : " + ex.Message);
                }
            });
            this.Nodes.ToList().FindAll(x => x.Value.NodeBlockType == NodeTypeEnum.Connector).ForEach(x =>
            {
                try
                {
                    x.Value.OnStop();
                }
                catch(Exception ex)
                {
                    this.AppendLog("error", "Can't release the event " + x.Value.FriendlyName + " : " + ex.Message);
                }
            });

            try
            {
                this.queueTaskCycleThreads.ToList().ForEach(x => x.Value.Dispose());
                if(this.cancelCycleToken != null) this.cancelCycleToken.Cancel();
            }
            catch(Exception ex)
            {
                this.AppendLog("error", "Internal error : " + ex.Message);
            }
            logger.Info("Stop requested for graph hash {0}", this.UniqueHash);
            this.AppendLog("warn", string.Format("Stop requested for graph hash {0}", this.UniqueHash));

            IsRunning = false;
            GraphsContainer.StopGraphByHash(this.UniqueHash);
            return true;
        }

        public void AddNode(Node node)
        {
            if(node.Id == string.Empty)
            {
                string guid = Guid.NewGuid().ToString();
                node.Id = guid;
            }
            this.Nodes.Add(node.Id, node);
        }

        public string ExportGraph()
        {
            var schema = new BlockGraphSchema(this);
            return schema.Export();
        }

        public void ExportGraphToFile(string filePath)
        {
            File.WriteAllText(filePath, this.ExportGraph());
        }

        public static BlockGraph LoadGraphFromFile(string filePath)
        {
            string rawJson = File.ReadAllText(filePath);
            string compressedRaw = GraphCompression.CompressGraphData(rawJson);
            string hash = GraphCompression.GetUniqueGraphHash(-1, compressedRaw);

            return LoadGraph(rawJson, hash, compressedRaw);
        }

        public static BlockGraph LoadGraph(string graphJson,
            string uniqueHash, string compressedRaw)
        {
            var graphSchema = JsonConvert.DeserializeObject<BlockGraphSchema>(graphJson);
            var graph = new BlockGraph(graphSchema.Name, null, false);
            graph.RawGraphData = graphJson;
            graph.UniqueHash = uniqueHash;
            graph.CompressedRaw = compressedRaw;

            if (string.IsNullOrEmpty(graph.UniqueHash) || string.IsNullOrEmpty(graph.CompressedRaw))
                throw new Exception("UniqueHash or CompressedRaw from BlockGraph cannot be null.");


            foreach (var rawDep in graphSchema.RawDeps)
            {
                var blockGraphDep = JsonConvert.DeserializeObject<BlockGraphSchema>(rawDep);
                foreach (var n in blockGraphDep.Nodes)
                {
                    graphSchema.Nodes.Add(n);
                }
            }

            // Load nodes
            foreach (var nodeSchema in graphSchema.Nodes)
            {
                Node node = null;
                var typeFromSchema = NodeBlockExporter.GetNodes().FirstOrDefault(x => x.NodeType == nodeSchema.Type);
                if(typeFromSchema != null)
                {
                    node = Activator.CreateInstance(typeFromSchema.GetType(), nodeSchema.Id, graph) as Node;
                }
                if (node != null) graph.Nodes.Add(node.Id, node);
            }

            // Load assignments and exec direction
            foreach (var nodeSchema in graphSchema.Nodes)
            {
                if (!graph.Nodes.ContainsKey(nodeSchema.Id)) continue;
                var node = graph.Nodes[nodeSchema.Id];
                if (nodeSchema.OutNode != null)
                {
                    if(graph.Nodes.ContainsKey(nodeSchema.OutNode)) node.OutNode = graph.Nodes[nodeSchema.OutNode];
                }
                foreach (var parameter in nodeSchema.InParameters)
                {
                    var nodeParam = node.InParameters[parameter.Name];
                    nodeParam.Id = parameter.Id;
                    nodeParam.Value = parameter.Value;
                }
                foreach (var parameter in nodeSchema.OutParameters)
                {
                    if (!node.OutParameters.ContainsKey(parameter.Name))
                        continue;
                    var nodeParam = node.OutParameters[parameter.Name];
                    nodeParam.Id = parameter.Id;
                    if(parameter.ValueIsReference && parameter.Value != null && parameter.Value.ToString() != "")
                    {
                        if(graph.Nodes.ContainsKey((string)parameter.Value))
                        {
                            nodeParam.Value = graph.Nodes[(string)parameter.Value];
                        }
                    }
                    else
                    {
                        nodeParam.Value = parameter.Value;
                    }
                }
            }
            foreach (var nodeSchema in graphSchema.Nodes)
            {
                if (!graph.Nodes.ContainsKey(nodeSchema.Id)) continue;
                var node = graph.Nodes[nodeSchema.Id];

                foreach (var parameter in nodeSchema.InParameters)
                {
                    var nodeParam = node.InParameters[parameter.Name];
                    if (parameter.Assignment != string.Empty)
                    {
                        nodeParam.Assignments = graph.FindParameterById(parameter.Assignment);
                    }
                }

                foreach (var parameter in nodeSchema.OutParameters)
                {
                    if (!node.OutParameters.ContainsKey(parameter.Name)) continue;
                    var nodeParam = node.OutParameters[parameter.Name];
                    if (parameter.Assignment != string.Empty)
                    {
                        nodeParam.Assignments = graph.FindParameterById(parameter.Assignment);
                    }
                }
            }

            //foreach(var rawDep in graphSchema.RawDeps)
            //{
            //    var blockGraphDep = LoadGraph(rawDep, "0x0", rawDep);
            //    foreach (var n in blockGraphDep.Nodes)
            //    {
            //        graph.Nodes.Add(n.Key, n.Value);
            //    }
            //}

            return graph;
        }

        public NodeParameter FindParameterById(string id)
        {
            foreach (var node in this.Nodes)
            {
                foreach (var parameter in node.Value.OutParameters)
                {
                    if (parameter.Value.Id == id)
                    {
                        return parameter.Value;
                    }
                }
            }
            return null;
        }

        public Node FindParameterNodeById(string id)
        {
            foreach (var node in this.Nodes)
            {
                foreach (var parameter in node.Value.OutParameters)
                {
                    if (parameter.Value.Id == id)
                    {
                        return node.Value;
                    }
                }
            }
            return null;
        }

        public EntryPointNode GetEntryPointNode()
        {
            return this.Nodes.FirstOrDefault(x => x.Value.NodeType == typeof(EntryPointNode).Name).Value as EntryPointNode;
        }

        public bool ExecuteFromEntryPoint()
        {
            if (this.GetEntryPointNode() == null) return false;
            this.AddCycle(this.GetEntryPointNode());
            return true;
        }

        public string GetId()
        {
            return this.UniqueHash;
        }

        public void Start(GraphContextWrapper context)
        {
            this.StartedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            this.currentContext = context;
            this.MemoryVariables = new Dictionary<string, object>();
            GraphsContainer.UpdateStorageStateGraph(context, Enums.GraphStateEnum.STARTED);

            // Init connectors
            foreach(var x in this.Nodes.ToList().FindAll(x => x.Value.NodeBlockType == NodeTypeEnum.Connector))
            {
                try
                {
                    currentCycle = new GraphExecutionCycle(this, DateTimeOffset.Now.ToUnixTimeSeconds(), x.Value, new Dictionary<string, NodeParameter>());
                    x.Value.SetupConnector();
                }
                catch (Exception ex)
                {
                    this.AppendLog("error", "Can't setup the connector : " + x.Value.FriendlyName + ", " + ex.Message);
                    this.Stop(true);
                    return;
                }
            }

            if (this.Nodes.ToList().FindAll(x => x.Value.IsEventNode).Count == 0)
            {
                try
                {
                    IsRunning = true;
                    this.ExecuteFromEntryPoint();
                    return;
                }
                catch (Exception ex)
                {
                    this.AppendLog("error", "Error when executing the graph : " + ex.Message);
                    this.Stop(true);
                    return;
                }
            }

            IsRunning = true;
            foreach (var x in this.Nodes.ToList().FindAll(x => x.Value.IsEventNode).OrderByDescending(x => x.Value.GetType() == typeof(OnGraphStartNode)))
            {
                try
                {
                    currentCycle = new GraphExecutionCycle(this, DateTimeOffset.Now.ToUnixTimeSeconds(), x.Value, new Dictionary<string, NodeParameter>());
                    x.Value.SetupEvent();
                }
                catch (Exception ex)
                {
                    this.AppendLog("error", "Can't setup the event : " + x.Value.FriendlyName + ", " + ex.Message); 
                    this.Stop(true);
                    return;
                }
            }
            this.ExecuteFromEntryPoint();
        }

        public BigInteger GetGraphGasExecutionTotal()
        {
            BigInteger total = new BigInteger();
            this.Nodes.ToList().ForEach(x =>
            {
                if (x.Value.GetType().GetCustomAttributes(typeof(NodeGasConfiguration), true).Length == 0) return;
                var gasConfig = x.Value.GetType().GetCustomAttributes(typeof(NodeGasConfiguration), true)[0] as NodeGasConfiguration;
                total += gasConfig.BlockGasPrice;
            });
            return total;
        }

        public void AddCycle(Node startNode, Dictionary<string, NodeParameter> parameters = null, string cycleQueueInstance = "main")
        {
            if (startNode.LastCycleAt + startNode.NodeCycleLimit > DateTimeOffset.Now.ToUnixTimeMilliseconds()) return;
            startNode.LastCycleAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.AddCycleToQueue(cycleQueueInstance, new GraphExecutionCycle(this, DateTimeOffset.Now.ToUnixTimeSeconds(), startNode, parameters));
        }

        public GraphExecutionCycle GetCurrentCycle()
        {
            return this.currentCycle;
        }

        public bool CheckLogRotate()
        {
            if (RotateLastUpdate == null)
            {
                RotateLastUpdate = DateTime.UtcNow;
                return true;
            }
            else
            {
                var now = DateTime.UtcNow;
                TimeSpan difference = now.Subtract(RotateLastUpdate.Value);
                if (difference.TotalSeconds > 30)
                {
                    return true;
                }
            }
            return false;
        }

        public void AppendLog(string type, string message)
        {
            if(Environment.GetEnvironmentVariable("graph_env") == "dev")
            {
                logger.Debug("[{0}] {1}", type, message);
            }
            if (CheckLogRotate())
            {
                var currentLogs = StorageManager.GetStorage().GetLogsForGraph(this.UniqueHash);
                if (currentLogs.Count > 100)
                    currentLogs.RemoveRange(0, 50);
                currentLogs.Add(new Storage.Redis.Entities.LogEntry() { Type = type, Message = message, Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds() });
                StorageManager.GetStorage().SaveLogsEntries(this.UniqueHash, currentLogs);
            }
            else
            {
                StorageManager.GetStorage().AppendLogForGraph(this.UniqueHash, type, message);
            }
        }

        public Dictionary<string, object> CallFunctionWithNewCycle(string name, Dictionary<string, object> parameters)
        {

            var functionNode = this.Nodes.FirstOrDefault(x => x.Value.NodeType == "FunctionNode" &&
                x.Value.InParameters["name"].GetValue().ToString() == name).Value as FunctionNode;
            if (functionNode == null) return null;
            semaphoreSlim.Wait();
            //TODO: Verify if there is no cycle in pending to execute the function
            currentCycle = new GraphExecutionCycle(this, DateTimeOffset.Now.ToUnixTimeSeconds(), functionNode, new Dictionary<string, NodeParameter>());
                functionNode.CallParameters = parameters;
                functionNode.Execute();
            semaphoreSlim.Release();

            this.PreviousCycles.Push(currentCycle);
            this.currentCycle = null;
            return functionNode.Context.ReturnValues;
        }

        public bool HasHostedAPI()
        {
            return this.Nodes.ToList().FindAll(x => x.Value.NodeType == typeof(
            ExposeAPIBlockNode).Name).Count > 0;
        }

        public ExposeAPIBlockNode GetHostedAPI()
        {
            return this.Nodes.ToList().FirstOrDefault(x => x.Value.NodeType == typeof(ExposeAPIBlockNode).Name).Value as ExposeAPIBlockNode;
        }

        public List<FunctionNode> GetFunctions()
        {
            return this.Nodes.ToList().FindAll(x => x.Value.NodeType == typeof(FunctionNode).Name).Select(x => x.Value as FunctionNode).ToList();
        }
    }
}
