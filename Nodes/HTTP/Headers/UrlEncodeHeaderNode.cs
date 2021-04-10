using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
namespace NodeBlock.Engine.Nodes.HTTP.Headers
{
    [NodeDefinition("UrlEncodeHeaderNode", "Url Encode Header Node", NodeTypeEnum.Function, "HTTP")]
    [NodeGraphDescription("Convert the array key-value data into a header for a http request.")]
    public class UrlEncodeHeaderNode : Node
    {

        public UrlEncodeHeaderNode(string id, BlockGraph graph)
            : base(id, graph, typeof(UrlEncodeHeaderNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "array", new NodeParameter(this, "array", typeof(List<object>), true) },

            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "httpContent", new NodeParameter(this, "httpContent", typeof(HttpContent), false, null, "", true) },
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var values = ((List<object>)this.InParameters["array"].GetValue()).Select(x => new KeyValuePair<string, string>(((dynamic)x).Key, ((dynamic)x).Value)); ;
            var httpcontent = new FormUrlEncodedContent(values);
            
            this.OutParameters["httpContent"].SetValue(httpcontent);
            return true;
        }
    }
}
