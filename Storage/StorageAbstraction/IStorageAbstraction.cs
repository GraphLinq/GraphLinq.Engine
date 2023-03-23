using NodeBlock.Engine.Enums;
using NodeBlock.Engine.Storage.Redis.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Storage.StorageAbstraction
{
    public interface IStorageAbstraction
    {
        GraphStorage SetGraphStorage(BlockGraph graph, int walletIdentifier, GraphStateEnum stateGraph);
        GraphStorage GetGraphStorage(string hash);
        void SetGraphKeyItem(BlockGraph graph, string key, string value);
        void SetWalletGraphKeyItem(BlockGraph graph, string key, string value);
        string GetGraphKeyItem(BlockGraph graph, string key);
        string GetWalletGraphKeyItem(BlockGraph graph, string key);
        bool GraphKeyItemExist(BlockGraph graph, string key);
        bool GraphWalletKeyItemExist(BlockGraph graph, string key);
        void SaveListActiveGraphs(List<BlockGraph> graphs);
        List<GraphStorage> GetGraphStorages();
        void AppendLogForGraph(string hash, string type, string message);
        void SaveLogsEntries(string hash, List<LogEntry> logs);
        List<LogEntry> GetLogsForGraph(string graphId);
        void RemoveGraphLogs(string hashGraph);
    }
}
