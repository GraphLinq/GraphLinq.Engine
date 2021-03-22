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
            JSON = 1
        }

        public RequestContext(HttpContext context, string rawBody)
        {
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

        private void parseRawBody()
        {
            if (string.IsNullOrEmpty(this.RawBody)) return;
            this.RequestBodyObject = JObject.Parse(this.RawBody);
        }

        public async Task<bool> AwaitResponse()
        {
            timeoutTask = Task.Delay(10000);
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
