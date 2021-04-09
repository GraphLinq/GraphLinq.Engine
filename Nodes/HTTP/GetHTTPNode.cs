﻿using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Engine.Nodes.HTTP
{
    [NodeDefinition("GetHTTPNode", "Get HTTP Request", NodeTypeEnum.Function, "HTTP")]
    [NodeGraphDescription("Make an HTTP GET request to any requested server")]
    public class GetHTTPNode : Node
    {
        private HttpClient client = new HttpClient();

        public GetHTTPNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetHTTPNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "url", new NodeParameter(this, "url", typeof(string), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "content", new NodeParameter(this, "content", typeof(string), false, null, "", true) },
                { "exception", new NodeParameter(this, "exception", typeof(Node), false, null, "", true) }

            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            try
            {
                var requestUrl = client.GetAsync((string)this.InParameters["url"].GetValue());
                requestUrl.Wait(1000);

                var responseString = requestUrl.Result.Content.ReadAsStringAsync();
                responseString.Wait(1000);
                this.OutParameters["content"].Value = responseString.Result;
            }
            catch(Exception ex)
            {
                if (this.OutParameters["exception"].Value != null)
                {
                    return (this.OutParameters["exception"].Value as Node).Execute();
                }
            }

            return true;

        }
    }
}