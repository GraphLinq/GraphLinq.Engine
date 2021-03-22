using Newtonsoft.Json.Linq;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{

    [NodeDefinition("JsonSelectorNode", "JSON Selector", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Select a specific value in a json object and return it as string parameter")]
    public class JsonSelectorNode : Node
    {
        public JsonSelectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(JsonSelectorNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "json", new NodeParameter(this, "json", typeof(string), true) },
                { "selector", new NodeParameter(this, "selector", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(string), false, null, "", true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var value = JObject.Parse((string)this.InParameters["json"].GetValue()).SelectToken((string)this.InParameters["selector"].GetValue());
            this.OutParameters["value"].Value = value.ToString();
            return true;
        }
    }
}
