using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringContainsNode", "String Contains", NodeTypeEnum.Condition, "String")]
    [NodeGraphDescription("Check if a string contains a other string")]
    public class StringContainsNode : Node
    {
        public StringContainsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringContainsNode).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));
            this.InParameters.Add("toSearch", new NodeParameter(this, "toSearch", typeof(string), true));

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
            var result = this.InParameters["string"].GetValue().ToString().ToLower().Contains(this.InParameters["toSearch"].GetValue().ToString().ToLower());
            if (result)
            {
                if (this.OutParameters["true"].Value != null)
                {
                    return (this.OutParameters["true"].Value as Node).Execute();
                }
            }
            else
            {
                if (this.OutParameters["false"].Value != null)
                {
                    return (this.OutParameters["false"].Value as Node).Execute();
                }
            }
            return true;
        }
    }
}
