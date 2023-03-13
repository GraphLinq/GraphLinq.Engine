using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StartWithNode", "String Start With", NodeTypeEnum.Condition, "String")]
    [NodeGraphDescription("Trigger different node path on a condition based on characters at the start of a string")]
    public class StartWithNode : Node
    {
        public StartWithNode(string id, BlockGraph graph)
           : base(id, graph, typeof(StartWithNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "text", new NodeParameter(this, "text", typeof(string), true) },
                { "startText", new NodeParameter(this, "startText", typeof(string), true) }
            };
            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "true", new NodeParameter(this, "true", typeof(Node), false) },
                { "false", new NodeParameter(this, "false", typeof(Node), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.InParameters["text"].GetValue().ToString().StartsWith(this.InParameters["startText"].GetValue().ToString()))
            {
                if (this.OutParameters["true"].Value == null) return true;
                return (this.OutParameters["true"].Value as Node).Execute();
            }
            else
            {
                if (this.OutParameters["false"].Value == null) return true;
                return (this.OutParameters["false"].Value as Node).Execute();
            }
        }
    }
}
