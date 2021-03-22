using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("FetchHTTPNode", "HTTP Request", NodeTypeEnum.Function, "HTTP")]
    [NodeGraphDescription("Make an HTTP GET request to any requested server")]
    public class FetchHTTPNode : Node
    {
        private HttpClient client = new HttpClient();

        public FetchHTTPNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchHTTPNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "url", new NodeParameter(this, "url", typeof(string), true) },
                { "method", new NodeParameter(this, "method", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "content", new NodeParameter(this, "content", typeof(string), false, null, "", true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            switch(this.InParameters["method"].GetValue().ToString().ToLower())
            {
                case "get":
                    var response = client.GetAsync((string)this.InParameters["url"].GetValue()).Result;
                    var resString = response.Content.ReadAsStringAsync().Result;
                    this.OutParameters["content"].Value = resString;
                    break;
            }
            return true;
        }
    }
}
