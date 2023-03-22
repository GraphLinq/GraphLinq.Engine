using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using NodeBlock.Engine.API.Services;
using NodeBlock.Engine.API.Entities;
using System;
using NodeBlock.Engine.Encoding;
using System.Linq;
using System.Collections.Generic;
using NodeBlock.Engine.Storage.Redis;

namespace NodeBlock.Engine.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GraphsController : ControllerBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IGraphService _graphService;

        public GraphsController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpPost("state")]
        public IActionResult UpdateGraphState([FromBody] Graph graph)
        {
            var graphContext = GraphsContainer.GetRunningGraphByHash(graph.UniqueHash);
            if (graphContext == null)
                return Ok(new { success = false, message = string.Format("Graph {0} not loaded in the GraphLinq engine", graph.UniqueHash) });

            GraphsContainer.UpdateStorageStateGraph(graphContext, graph.DeployedState);
            if (graph.DeployedState == Enums.GraphStateEnum.STARTING || graph.DeployedState == Enums.GraphStateEnum.RESTARTING)
            {
                var decompressedRaw = GraphCompression.DecompressGraphData(graphContext.graph.CompressedRaw);
                var reloadedGraph = BlockGraph.LoadGraph(decompressedRaw, graph.UniqueHash, graphContext.graph.CompressedRaw);

                GraphsContainer.AddNewGraph(reloadedGraph, graphContext.graph.currentContext.walletIdentifier, graph.DeployedState);
            }
            return Ok(new { success = true });
        }

        [HttpPost("deploy")]
        public async Task<IActionResult> DeployAndInitGraph([FromBody] Graph graph)
        {
            if (graph.Debug)
            {
                var debugGraph = GraphsContainer.GetWalletDebugGraph(graph.WalletIdentifier);
                if (debugGraph != null)
                {
                    graph.UniqueHash = debugGraph.graph.UniqueHash;
                    RedisStorage.RemoveGraphLogs(graph.UniqueHash);
                }
            }

            BlockGraph loadedGraph = await _graphService.InitGraph(graph);
            if (loadedGraph == null)
                return StatusCode(500);
            GraphsContainer.AddNewGraph(loadedGraph, graph.WalletIdentifier, graph.DeployedState);

            return Ok(new { hash = loadedGraph.UniqueHash });
        }

        [HttpPost("compress")]
        public IActionResult CompressGraphsNodes([FromBody] GraphRaw raw)
        {
            try
            {
                var compressed = GraphCompression.CompressGraphData(raw.JsonData);
                return Ok(new
                {
                    compressed,
                    hash = GraphCompression.GetUniqueGraphHash(raw.WalletIdentifier, compressed)
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpPost("decompress")]
        public IActionResult DecompressGraphsNodes([FromBody] GraphRaw raw)
        {
            try
            {
                var decompressed = GraphCompression.DecompressGraphData(raw.JsonData);
                return Ok(new
                {
                    decompressed,
                    hash = GraphCompression.GetUniqueGraphHash(raw.WalletIdentifier, raw.JsonData)
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpPost("infos")]
        public IActionResult GetGraphInfos([FromBody] Graph raw)
        {
            try
            {
                GraphContextWrapper graph = null;
                if (raw.Debug)
                    graph = GraphsContainer.GetWalletDebugGraph(raw.WalletIdentifier);
                else
                    graph = GraphsContainer.GetRunningGraphByHash(raw.UniqueHash);
                    

                if (graph == null)
                    return StatusCode(404);
                var hostedApiEndpoints = new List<string>();
                if(graph.graph.HasHostedAPI())
                {
                    foreach(var endpoint in graph.graph.GetHostedAPI().HostedAPI.Endpoints)
                    {
                        hostedApiEndpoints.Add(endpoint.Key);
                    }
                }

                return Ok(new
                {
                    state = graph.currentGraphState,
                    loadedAt = graph.LoadedAt,
                    stoppedAt = graph.StoppedAt,
                    cycleCount = graph.graph.CycleCountSinceStart,
                    hostedApi = graph.graph.HasHostedAPI(),
                    hostedApiEndpoints = hostedApiEndpoints,
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpPost("infos/lists")]
        public IActionResult GetGraphsInfos([FromBody] Graph[] raw)
        {
            try
            {
                List<object> infos = new List<object>();

                raw.ToList().ForEach(x =>
                {
                    var graph = GraphsContainer.GetRunningGraphByHash(x.UniqueHash);
                    if (graph == null) { return; }
                    infos.Add(new
                    {
                        hostedApi = graph?.graph.HasHostedAPI(),
                        hash = x.UniqueHash,
                        state = graph.currentGraphState,
                        loadedAt = graph.LoadedAt,
                        stoppedAt = graph.StoppedAt
                    });
                });
                
                return Ok(infos);
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpPost("logs")]
        public IActionResult GetGraphLogs([FromBody] Graph raw)
        {
            try
            {
                var logs = Storage.Redis.RedisStorage.GetLogsForGraph(raw.UniqueHash);
                return Ok(new
                {
                    logs = logs
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpGet("healthcheck")]
        public IActionResult HealthCheck()
        {
            try
            {
                return Ok(new
                {
                    alive = true
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpGet("metrics")]
        public IActionResult Metrics()
        {
            try
            {
                var graphs = GraphsContainer.GetGraphs();

                return Ok(new
                {
                    engine = new
                    {
                        started_at = GraphsContainer.StartAt
                    },
                    metrics = new
                    {
                        total_graphs = graphs.Count
                    }
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpGet("graph_list")]
        public IActionResult GraphList()
        {
            try
            {
                var graphs = GraphsContainer.GetGraphs();
                return Ok(new
                {
                    graphs = graphs.Select(x => new
                    {
                        name = x.Value.graph.Name,
                        hash = x.Value.graph.UniqueHash,
                        wallet_identifier = x.Value.walletIdentifier,
                        state = x.Value.currentGraphState,
                        loaded_at = x.Value.LoadedAt
                    })
                }); ;
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }

        [HttpPost("trace")]
        public IActionResult GetGraphTraces([FromBody] Graph raw)
        {
            try
            {
                var graph = GraphsContainer.GetRunningGraphByHash(raw.UniqueHash);
                if (graph == null)
                {
                    return StatusCode(404);
                }
                var traces = graph.graph.PreviousCycles.Select(x => new
                {
                    start_at = x.Timestamp,
                    trace_start_node = x.StartNode.FriendlyName,
                    execution_duration = x.Trace.GetExecutionTime(),
                    execution_success = x.Trace.GetExecutionSuccess(),
                    execution_exception = x.Trace.Exception == null ? string.Empty : x.Trace.Exception.ToString(),
                    stack = x.Trace.Stack.Select(y => new
                    {
                        id = y.NodeId,
                        execution_node_duration = y.ExecutionTime,
                        node_type = y.Node.NodeType,
                        execution_node_exception = y.ExecutionException == null ? string.Empty : y.ExecutionException.ToString(),
                        parameters = y.Parameters.Select(z => new
                        {
                            key = z.Key,
                            value = z.Value
                        })
                    })
                });

                return Ok(new
                {
                    traces = traces
                });
            }
            catch (Exception error)
            {
                logger.Error(error);
                return StatusCode(500);
            }
        }
    }
}
