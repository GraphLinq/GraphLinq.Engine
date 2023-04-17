using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Engine.HostedAPI
{
    public class RequestContext
    {
        public enum ResponseFormatTypeEnum
        {
            JSON = 1,
            HTML = 2,
            JS = 3,
            CSS = 4
        }

        public RequestContext( HttpContext context, string rawBody, BlockGraph graph, int customTimeout)
        {
            this.graph = graph;
            this.customTimeout = customTimeout;
            Context = context;
            RawBody = rawBody;
            this.parseRawBody();
        }

        public HttpContext Context { get; }
        public string RawBody { get; }
        public JObject RequestBodyObject { get; set; }

        private TaskCompletionSource<bool> completedTask = new System.Threading.Tasks.TaskCompletionSource<bool>();
        private Task timeoutTask = null;
        public string Body { get; set; }
        public ResponseFormatTypeEnum ResponseFormatType = ResponseFormatTypeEnum.JSON;
        private readonly BlockGraph graph;
        private readonly int customTimeout;

        private void parseRawBody()
        {
            if (string.IsNullOrEmpty(this.RawBody)) return;
            this.RequestBodyObject = JObject.Parse(this.RawBody);
        }

        public async Task<bool> AwaitResponse()
        {
            timeoutTask = Task.Delay(this.customTimeout);
            var result = await Task.WhenAny(completedTask.Task, timeoutTask);
            if(result == timeoutTask)
            {
                return false;
            }
            return completedTask.Task.Result;
        }

        public void Complete(bool result)
        {
            completedTask.SetResult(result);
        }
    }
}
