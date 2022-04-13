using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringContainsMultiNode", "String Contains Multiple", NodeTypeEnum.Condition, "String")]
    [NodeGraphDescription("Check if a string contains any item in array")]
    [NodeIDEParameters(Hidden = true)]
    public class StringContainsMultiNode : Node
    {
        public StringContainsMultiNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringContainsMultiNode).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));
            this.InParameters.Add("searchItems", new NodeParameter(this, "searchItems", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "true", new NodeParameter(this, "true", typeof(Node), false) },
                { "false", new NodeParameter(this, "false", typeof(Node), false) }
            };
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => false;

        public override bool OnExecution()
        {
            var original = this.InParameters["string"].GetValue().ToString();
            var items = this.InParameters["searchItems"].GetValue().ToString().Split(",");
            foreach (var item in items)
            {
                if (original == item)
                {
                    return (this.OutParameters["true"].Value as Node).Execute();
                }    
            }

            return (this.OutParameters["false"].Value as Node).Execute();
        }
    }
}
