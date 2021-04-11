using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Engine.Nodes.HTTP
{
    [NodeDefinition("DeleteHTTPNode", "Delete HTTP Request", NodeTypeEnum.Function, "HTTP")]
    [NodeGraphDescription("Make an HTTP Delete request to any requested server")]
    public class DeleteHTTPNode : Node
    {
        public DeleteHTTPNode(string id, BlockGraph graph)
            : base(id, graph, typeof(DeleteHTTPNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "url", new NodeParameter(this, "url", typeof(string), true) },
                { "headers", new NodeParameter(this, "headers", typeof(List<object>), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "result", new NodeParameter(this, "result", typeof(string), false, null, "", true) },
                { "exception", new NodeParameter(this, "exception", typeof(Node), false, null, "", true) }

            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (this.InParameters["headers"].GetValue() != null)
                    {
                        foreach (var header in (List<object>)this.InParameters["headers"].GetValue())
                        {
                            client.DefaultRequestHeaders.Add(((dynamic)header).Key, ((dynamic)header).Value);
                        }
                    }

                    var requestUrl = client.GetAsync((string)this.InParameters["url"].GetValue());
                    requestUrl.Wait(1000);

                    var responseString = requestUrl.Result.Content.ReadAsStringAsync();
                    responseString.Wait(1000);
                    this.OutParameters["result"].Value = responseString.Result;
                }           
            }
            catch (Exception ex)
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
