using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodeBlock.Engine.API.Entities;
using NodeBlock.Engine.Encoding;
using System;

namespace NodeBlock.Engine.API.Services
{
    public interface IGraphService
    {
        Task<BlockGraph> InitGraph(Graph grap);
    }

    public class GraphService : IGraphService
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<BlockGraph> InitGraph(Graph graph)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var hash = graph.UniqueHash ?? GraphCompression.GetUniqueGraphHash(graph.WalletIdentifier, graph.RawBytes);
                    var decompressedRaw = GraphCompression.DecompressGraphData(graph.RawBytes);
                    var loadedGraph = BlockGraph.LoadGraph(decompressedRaw, hash, graph.RawBytes);
                    loadedGraph.Debug = graph.Debug;

                    return loadedGraph;
                }
                catch (Exception error)
                {
                    logger.Error(error);
                    return null;
                }
            });
       
        }
    }

}
