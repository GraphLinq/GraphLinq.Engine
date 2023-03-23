using NodeBlock.Engine.Enums;
using NodeBlock.Engine.Storage.Redis.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using Newtonsoft.Json;
using System.Linq;
using NodeBlock.Engine.Encoding;

namespace NodeBlock.Engine.Storage.StorageAbstraction.LocalStorage
{
    public class LocalStorage : IStorageAbstraction
    {
        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        private LiteDatabase _db;

        public LocalStorage(string dbFilePath)
        {
            _db = new LiteDatabase(dbFilePath);
        }

        public void AppendLogForGraph(string hash, string type, string message)
        {
            var logStore = _db.GetCollection<LogEntry>("graphLogs");
            var logs = logStore.Find(x => x.GraphHash == hash).ToList();

            var logEntry = new LogEntry()
            {
                GraphHash = hash,
                Type = type,
                Message = message,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            logs.Add(logEntry);
            logStore.Upsert(logEntry);
        }

        public string GetGraphKeyItem(BlockGraph graph, string key)
        {
            var keyValueStore = _db.GetCollection<KeyValue>("graphKeyItems");
            var isolatedKey = graph.GetId() + "/" + key;
            var result = keyValueStore.FindOne(x => x.Key == isolatedKey);
            return result?.Value;
        }

        public GraphStorage GetGraphStorage(string hash)
        {
            var keyValueStore = _db.GetCollection<GraphStorage>("graphs");
            return keyValueStore.FindOne(x => x.StoredHash == hash);
        }

        public List<GraphStorage> GetGraphStorages()
        {
            var graphStore = _db.GetCollection<GraphStorage>("graphs");
            var activeGraphsStore = _db.GetCollection<ActiveGraphStorage>("activeGraphs");
            var activeGraphs = activeGraphsStore.FindAll().ToList();

            return graphStore.Find(x => activeGraphs.Any(ag => ag.Hash == x.StoredHash)).ToList();
        }

        public List<LogEntry> GetLogsForGraph(string graphId)
        {
            var logStore = _db.GetCollection<LogEntry>("graphLogs");
            return logStore.Find(x => x.GraphHash == graphId).ToList();
        }

        public string GetWalletGraphKeyItem(BlockGraph graph, string key)
        {
            var keyValueStore = _db.GetCollection<KeyValue>("walletGraphKeyItems");
            var isolatedKey = graph.currentContext.walletIdentifier + "/" + key;
            var result = keyValueStore.FindOne(x => x.Key == isolatedKey);
            return result?.Value;
        }

        public bool GraphKeyItemExist(BlockGraph graph, string key)
        {
            var keyValueStore = _db.GetCollection<KeyValue>("graphKeyItems");
            var isolatedKey = graph.GetId() + "/" + key;
            return keyValueStore.Exists(x => x.Key == isolatedKey);
        }

        public bool GraphWalletKeyItemExist(BlockGraph graph, string key)
        {
            var keyValueStore = _db.GetCollection<KeyValue>("walletGraphKeyItems");
            var isolatedKey = graph.currentContext.walletIdentifier + "/" + key;
            return keyValueStore.Exists(x => x.Key == isolatedKey);
        }

        public void RemoveGraphLogs(string hashGraph)
        {
            var logStore = _db.GetCollection<LogEntry>("graphLogs");
            logStore.DeleteMany(x => x.GraphHash == hashGraph);
        }

        public void SaveListActiveGraphs(List<BlockGraph> graphs)
        {
            var activeGraphsStore = _db.GetCollection<ActiveGraphStorage>("activeGraphs");
            activeGraphsStore.DeleteMany(_ => true);
            activeGraphsStore.InsertBulk(graphs.Where(x => x.currentContext != null).Select(x => new ActiveGraphStorage()
            {
                Hash = x.UniqueHash,
                GraphLastState = x.currentContext.currentGraphState
            }));
        }

        public void SaveLogsEntries(string hash, List<LogEntry> logs)
        {
            var logStore = _db.GetCollection<LogEntry>("graphLogs");
            logStore.DeleteMany(x => x.GraphHash == hash);
            logs.ForEach(log => log.GraphHash = hash);
            logStore.InsertBulk(logs);
        }

        public void SetGraphKeyItem(BlockGraph graph, string key, string value)
        {
            var keyValueStore = _db.GetCollection<KeyValue>("graphKeyItems");
            var isolatedKey = graph.GetId() + "/" + key;
            keyValueStore.Upsert(new KeyValue { Key = isolatedKey, Value = value });
        }

        public GraphStorage SetGraphStorage(BlockGraph graph, int walletIdentifier, GraphStateEnum stateGraph)
        {
            string hash = GraphCompression.GetUniqueGraphHash(walletIdentifier, graph.CompressedRaw);
            var storage = new GraphStorage()
            {
                StoredHash = hash,
                CompressedBytes = graph.CompressedRaw,
                WalletIdentifierOwner = walletIdentifier,
                StateGraph = stateGraph
            };

            var keyValueStore = _db.GetCollection<GraphStorage>("graphs");
            keyValueStore.Upsert(storage);

            return storage;
        }

        public void SetWalletGraphKeyItem(BlockGraph graph, string key, string value)
        {
            var keyValueStore = _db.GetCollection<KeyValue>("walletGraphKeyItems");
            var isolatedKey = graph.currentContext.walletIdentifier + "/" + key;
            keyValueStore.Upsert(new KeyValue { Key = isolatedKey, Value = value });
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
