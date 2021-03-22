using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Branch
{
    [NodeDefinition("StringBranchNode", "String Branch", NodeTypeEnum.Condition, "Base Condition")]
    [NodeGasConfiguration("1000000000000")]
    [NodeGraphDescription("Trigger a condition over two string value to compare if their are equals to each other")]
    public class StringBranchNode : Node
    {
        public StringBranchNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringBranchNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "valueA", new NodeParameter(this, "valueA", typeof(bool), true) },
                { "valueB", new NodeParameter(this, "valueB", typeof(bool), true) }
            };
            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "==", new NodeParameter(this, "==", typeof(Node), false) },
                { "!=", new NodeParameter(this, "!=", typeof(Node), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.OutParameters["=="].Value != null)
            {
                if (this.InParameters["valueA"].GetValue().ToString() == this.InParameters["valueB"].GetValue().ToString())
                {
                    return (this.OutParameters["=="].Value as Node).Execute();
                }
            }

            if (this.OutParameters["!="].Value != null)
            {
                if (this.InParameters["valueA"].GetValue().ToString() != this.InParameters["valueB"].GetValue().ToString())
                {
                    return (this.OutParameters["!="].Value as Node).Execute();
                }
            }

            return true;
        }
    }
}
