using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Branch
{
    [NodeDefinition("BoolBranchNode", "Boolean Branch", NodeTypeEnum.Condition, "Base Condition")]
    [NodeGraphDescription("Trigger different node path on a condition based on a bool variable value state (true/false).")]
    public class BoolBranchNode : Node
    {
        public BoolBranchNode(string id, BlockGraph graph)
            : base(id, graph, typeof(BoolBranchNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "condition", new NodeParameter(this, "condition", typeof(bool), true) }
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
            if(bool.Parse(this.InParameters["condition"].GetValue().ToString()))
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
