using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NodeBlock.Engine.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HostedAPIController : ControllerBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet("id/{graphId}/web/{*graphEndpoint}")]
        public async Task<IActionResult> RequestHostedGraphPublicPage(string graphId, string graphEndpoint)
        {
            try
            {
                var graphContext = GraphsContainer.GetRunningGraphByHash(graphId);
                if (graphContext == null)
                    return BadRequest(new { success = false, message = string.Format("Graph {0} not loaded in the GraphLinq engine", graphId) });

                if (!graphContext.graph.HasHostedAPI())
                    return BadRequest(new { success = false, message = string.Format("Graph {0} doesn't have a hosted API", graphId) });

                var graphHostedApi = graphContext.graph.GetHostedAPI().HostedAPI;
                if(graphEndpoint != null)
                {
                    if (!graphHostedApi.Endpoints.ContainsKey(graphEndpoint))
                        return BadRequest(new { success = false, message = string.Format("Graph {0} doesn't have this endpoint available", graphId) });
                }

                HostedEndpoint endpoint = null;
                if (graphEndpoint != null)
                {
                    endpoint = graphHostedApi.Endpoints[graphEndpoint];
                }
                else
                {
                    endpoint = graphHostedApi.Endpoints[""];
                }

                var context = await endpoint.OnRequest(HttpContext, string.Empty);
                if(endpoint.ContentType == "application/json" && !context.Body.Trim().StartsWith("{"))
                {
                    // Since it's not api we need to switch the content to html
                    context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.HTML;
                }
                else
                {
                    switch(endpoint.ContentType)
                    {
                        case "text/html":
                            context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.HTML;
                            break;

                        case "application/javascript":
                            context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.JS;
                            break;

                        case "text/css":
                            context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.CSS;
                            break;
                    }
                }
                if (context == null) return BadRequest();

                switch (context.ResponseFormatType)
                {
                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.JSON:
                        return Content(context.Body, "application/json");

                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.JS:
                        return Content(context.Body, "application/javascript");

                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.CSS:
                        return Content(context.Body, "text/css");

                    default:
                        return Content(context.Body, "text/html");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return BadRequest("Unknown error");
            }
        }

        [HttpPost("id/{graphId}/{*graphEndpoint}")]
        public async Task<IActionResult> RequestHostedGraphAPI(string graphId, string graphEndpoint)
        {
            try
            {
                var graphContext = GraphsContainer.GetRunningGraphByHash(graphId);
                if (graphContext == null)
                    return BadRequest(new { success = false, message = string.Format("Graph {0} not loaded in the GraphLinq engine", graphId) });

                if (!graphContext.graph.HasHostedAPI())
                    return BadRequest(new { success = false, message = string.Format("Graph {0} doesn't have a hosted API", graphId) });

                var graphHostedApi = graphContext.graph.GetHostedAPI().HostedAPI;
                if (!graphHostedApi.Endpoints.ContainsKey(graphEndpoint))
                    return BadRequest(new { success = false, message = string.Format("Graph {0} doesn't have this endpoint available", graphId) });

                var endpoint = graphHostedApi.Endpoints[graphEndpoint];
                var rawBody = "";
                using (StreamReader reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8))
                {
                    rawBody = await reader.ReadToEndAsync();
                }

                var context = await endpoint.OnRequest(HttpContext, rawBody);
                if (context == null) return BadRequest();

                switch (context.ResponseFormatType)
                {
                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.JSON:
                        return Content(context.Body, "application/json");

                    default:
                        return Ok(context.Body);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return BadRequest("Unknown error");
            }
        }

        [HttpGet("name/{graphName}/{*graphEndpoint}")]
        public async Task<IActionResult> RequestNamedHostedGraphPublicPage(string graphName, string graphEndpoint)
        {
            try
            {
                var graphContext = GraphsContainer.GetRunningGraphByName(graphName);
                if (graphContext == null)
                    return BadRequest(new { success = false, message = string.Format("Graph {0} not loaded in the GraphLinq engine", graphName) });

                if (!graphContext.graph.HasHostedAPI())
                    return BadRequest(new { success = false, message = string.Format("Graph {0} doesn't have a hosted API", graphName) });

                var graphHostedApi = graphContext.graph.GetHostedAPI().HostedAPI;
                if (graphEndpoint != null)
                {
                    if (!graphHostedApi.Endpoints.ContainsKey(graphEndpoint))
                        return BadRequest(new { success = false, message = string.Format("Graph {0} doesn't have this endpoint available", graphName) });
                }

                HostedEndpoint endpoint = null;
                if (graphEndpoint != null)
                {
                    endpoint = graphHostedApi.Endpoints[graphEndpoint];
                }
                else
                {
                    endpoint = graphHostedApi.Endpoints[""];
                }

                var context = await endpoint.OnRequest(HttpContext, string.Empty);
                if (endpoint.ContentType == "application/json" && !context.Body.Trim().StartsWith("{"))
                {
                    // Since it's not api we need to switch the content to html
                    context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.HTML;
                }
                else
                {
                    switch (endpoint.ContentType)
                    {
                        case "text/html":
                            context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.HTML;
                            break;

                        case "application/javascript":
                            context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.JS;
                            break;

                        case "text/css":
                            context.ResponseFormatType = HostedAPI.RequestContext.ResponseFormatTypeEnum.CSS;
                            break;
                    }
                }
                if (context == null) return BadRequest();

                switch (context.ResponseFormatType)
                {
                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.JSON:
                        return Content(context.Body, "application/json");

                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.JS:
                        return Content(context.Body, "application/javascript");

                    case HostedAPI.RequestContext.ResponseFormatTypeEnum.CSS:
                        return Content(context.Body, "text/css");

                    default:
                        return Content(context.Body, "text/html");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return BadRequest("Unknown error");
            }
        }
    }
}
