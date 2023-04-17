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
        public HostedEndpoint(HostedGraphAPI hostedGraphAPI, string route,string contentType, string servedFile = "")
        {
            HostedGraphAPI = hostedGraphAPI;
            Route = route;
            ContentType = contentType;
            ServedFile = servedFile;


            if(this.Route.EndsWith(".js"))
            {
                this.ContentType = "application/javascript";
            } else if (this.Route.EndsWith(".css"))
            {
                this.ContentType = "text/css";
            }
        }

        public HostedGraphAPI HostedGraphAPI { get; }
        public string Route { get; set; }
        public string ContentType { get; }
        public string ServedFile { get; }
        public OnEndpointRequestNode EventsNode { get; set; }
        public int CacheTTL = -1;
        public int CustomTimeout = 10000;
        public long LastResponseTime = -1;
        public string LastResponseCache = string.Empty;


        public async Task<RequestContext> OnRequest(HttpContext context, string rawBody)
        {
            var requestContext = new RequestContext(context, rawBody, this.HostedGraphAPI.Graph, this.CustomTimeout);
            if (EventsNode == null)
            {
                if (this.ServedFile == string.Empty) return null;
                requestContext.Body = this.ServedFile;
                requestContext.Complete(true);
                return requestContext;
            }
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            if(timestamp < LastResponseTime + this.CacheTTL && this.CacheTTL != -1 && this.LastResponseCache != string.Empty)
            {
                requestContext.Body = this.LastResponseCache;
                requestContext.Complete(true);
            }
            else
            {
                EventsNode.OnRequest(requestContext);
                var result = await requestContext.AwaitResponse();
                this.LastResponseCache = requestContext.Body;
                this.LastResponseTime = timestamp;
            }
            return requestContext;
        }
    }
}
