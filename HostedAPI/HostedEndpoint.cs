using Microsoft.AspNetCore.Http;
using NodeBlock.Engine.Nodes.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Engine.HostedAPI
{
    public class HostedEndpoint
    {
        public HostedEndpoint(HostedGraphAPI hostedGraphAPI, string route)
        {
            HostedGraphAPI = hostedGraphAPI;
            Route = route;
        }

        public HostedGraphAPI HostedGraphAPI { get; }
        public string Route { get; set; }
        public OnEndpointRequestNode EventsNode { get; set; }

        public async Task<RequestContext> OnRequest(HttpContext context, string rawBody)
        {
            var requestContext = new RequestContext(context, rawBody);
            if (EventsNode == null) return null;
            EventsNode.OnRequest(requestContext);
            var result = await requestContext.AwaitResponse();
            return requestContext;
        }
    }
}
