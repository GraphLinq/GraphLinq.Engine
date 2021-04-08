using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Engine.Nodes.HTTP
{

    [NodeDefinition("GetHTTPNode", "Get HTTP Request", NodeTypeEnum.Function, "HTTP")]
    [NodeGraphDescription("Make an HTTP Post request to any requested server")]
    public class PostHTTPNode : Node
    {
        private HttpClient client = new HttpClient();

        public PostHTTPNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetHTTPNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "url", new NodeParameter(this, "url", typeof(string), true) },
                {"data", new NodeParameter(this,"data",typeof(object),true,isDynamic:true) }
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

            return true;
        }
    }
}
