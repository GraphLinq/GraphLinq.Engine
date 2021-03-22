using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using NodeBlock.Engine.Storage.Redis.Entities;
using NodeBlock.Engine.Encoding;
using Newtonsoft.Json;
using NodeBlock.Engine.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace NodeBlock.Engine.Storage.Redis
{
    public class RedisStorage
    {
        private static ConnectionMultiplexer muxer;

        static RedisStorage()
        {
            muxer = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("redis_master_addr") + ":" + Environment.GetEnvironmentVariable("redis_master_port") + ",password=" +
                Environment.GetEnvironmentVariable("redis_master_passw") + ",connectTimeout=60000,syncTimeout=30000,asyncTimeout=30000"); ;
        }
        public static GraphStorage SetGraphStorage(BlockGraph graph, int walletIdentifier, GraphStateEnum stateGraph)
        {
            string hash = GraphCompression.GetUniqueGraphHash(walletIdentifier, graph.CompressedRaw);
            var storage = new GraphStorage()
            {
                StoredHash = hash,
                CompressedBytes = graph.CompressedRaw,
                WalletIdentifierOwner = walletIdentifier,
                StateGraph = stateGraph
            };

            var conn = muxer.GetDatabase(0);
            conn.StringSet(string.Format("graphs/{0}", hash), JsonConvert.SerializeObject(storage));
            return storage;
        }

        public static int GetGraphDatabaseId()
        {
            return Environment.GetEnvironmentVariable("graph_env") == "prod" ? 0 : 1;
        }

        public static GraphStorage GetGraphStorage(string hash)
        {
            var conn = muxer.GetDatabase(GetGraphDatabaseId());
            string rawContent = conn.StringGet(string.Format("graphs/{0}", hash));
            if (rawContent == null) { return null; }

            var storage = JsonConvert.DeserializeObject<GraphStorage>(rawContent);
            return storage;
        }

        public static void SetGraphKeyItem(BlockGraph graph, string key, string value)
        {
            var conn = muxer.GetDatabase(GetGraphDatabaseId());
            var isolatedKey = graph.GetId() + "/" + key;
            conn.StringSet(isolatedKey, value);
        }

        public static string GetGraphKeyItem(BlockGraph graph, string key)
        {
            var conn = muxer.GetDatabase(GetGraphDatabaseId());
            var isolatedKey = graph.GetId() + "/" + key;
            return conn.StringGet(isolatedKey);
        }

        public static bool GraphKeyItemExist(BlockGraph graph, string key)
        {
            var conn = muxer.GetDatabase(GetGraphDatabaseId());
            var isolatedKey = graph.GetId() + "/" + key;
            return conn.KeyExists(isolatedKey);
        }

        public static void SaveListActiveGraphs(List<BlockGraph> graphs)
        {
            var rawStates = JsonConvert.SerializeObject(graphs.Where(x => x.currentContext != null).Select(x => new ActiveGraphStorage()
            {
                Hash = x.UniqueHash,
                GraphLastState = x.currentContext.currentGraphState
            }).ToList());

            var conn = muxer.GetDatabase(GetGraphDatabaseId());
            conn.StringSet("graphs/active", rawStates);
        }

        public static List<GraphStorage> GetGraphStorages()
        {
            var graphsAlive = new List<GraphStorage>();
            var conn = muxer.GetDatabase(GetGraphDatabaseId());
            var rawActive = conn.StringGet("graphs/active");
            if (string.IsNullOrEmpty(rawActive)) { return graphsAlive; }

            ActiveGraphStorage[] lists = JsonConvert.DeserializeObject <ActiveGraphStorage[]>(rawActive);
            List<string> liveHashs = lists.Select(x => x.Hash).ToList();
            var transaction = conn.CreateTransaction();

            var tasks = new List<Task<RedisValue>>();
            liveHashs.ForEach(x => tasks.Add(transaction.StringGetAsync(string.Format("graphs/{0}", x))));
            transaction.Execute();

            tasks.ForEach(x =>
            {
                if (!x.Result.IsNullOrEmpty)
                {
                    var storage = JsonConvert.DeserializeObject<GraphStorage>(x.Result);
                    graphsAlive.Add(storage);
                }
            });

            return graphsAlive;
        }

        public static void AppendLogForGraph(string hash, string type, string message)
        {
            var key = "graph/logs/" + hash;
            var conn = muxer.GetDatabase(1);
            var currentValue = conn.StringGet(key);
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var logs = new List<Entities.LogEntry>();
            if (string.IsNullOrEmpty(currentValue))
            {
                logs.Add(new LogEntry()
                {
                    Type = type,
                    Message = message,
                    Timestamp = timestamp
                });
            }
            else
            {
                logs = JsonConvert.DeserializeObject<List<LogEntry>>(currentValue);
                logs.Add(new LogEntry()
                {
                    Type = type,
                    Message = message,
                    Timestamp = timestamp
                });
            }
            conn.StringSet(key, JsonConvert.SerializeObject(logs));
        }

        public static void SaveLogsEntries(string hash, List<LogEntry> logs)
        {
            var key = "graph/logs/" + hash;
            var conn = muxer.GetDatabase(1);
            conn.StringSet(key, JsonConvert.SerializeObject(logs));
        }

        public static List<Entities.LogEntry> GetLogsForGraph(string graphId)
        {
            var pattern = "graph/logs/" + graphId;
            var conn = muxer.GetDatabase(1);
            var raw = conn.StringGet(pattern);
            if (!raw.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<List<Entities.LogEntry>>(raw);
            }
            return new List<LogEntry>() { };
        }


        public static void RemoveGraphLogs(string hashGraph)
        {
            var conn = muxer.GetDatabase(1);
            conn.KeyDelete("graph/logs/" + hashGraph);
        }
    }
}
