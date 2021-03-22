using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Branch
{
    [NodeDefinition("DecimalBranchNode", "Decimal Branch", NodeTypeEnum.Condition, "Base Condition")]
    [NodeGraphDescription("Trigger different node path on a condition based on a decimal variable value state (equals to, greater or equals, lower then, lower or equals..).")]
    public class DecimalBranch : Node
    {
        public DecimalBranch(string id, BlockGraph graph)
            : base(id, graph, typeof(DecimalBranch).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "valueA", new NodeParameter(this, "valueA", typeof(double), true) },
                { "valueB", new NodeParameter(this, "valueB", typeof(double), true) }
            };
            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { ">", new NodeParameter(this, ">", typeof(Node), false) },
                { ">=", new NodeParameter(this, ">=", typeof(Node), false) },
                { "==", new NodeParameter(this, "==", typeof(Node), false) },
                { "<=", new NodeParameter(this, "<=", typeof(Node), false) },
                { "<", new NodeParameter(this, "<", typeof(Node), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.OutParameters[">"].Value != null && this.OutParameters[">"].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["valueA"].GetValue().ToString()) > double.Parse(this.InParameters["valueB"].GetValue().ToString()))
                {
                    return (this.OutParameters[">"].Value as Node).Execute();
                }
            }

            if (this.OutParameters[">="].Value != null && this.OutParameters[">="].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["valueA"].GetValue().ToString()) >= double.Parse(this.InParameters["valueB"].GetValue().ToString()))
                {
                    return (this.OutParameters[">="].Value as Node).Execute();
                }
            }

            if (this.OutParameters["=="].Value != null && this.OutParameters["=="].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["valueA"].GetValue().ToString()) == double.Parse(this.InParameters["valueB"].GetValue().ToString()))
                {
                    return (this.OutParameters["=="].Value as Node).Execute();
                }
            }

            if (this.OutParameters["<="].Value != null && this.OutParameters["<="].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["valueA"].GetValue().ToString()) <= double.Parse(this.InParameters["valueB"].GetValue().ToString()))
                {
                    return (this.OutParameters["<="].Value as Node).Execute();
                }
            }

            if (this.OutParameters["<"].Value != null && this.OutParameters["<"].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["valueA"].GetValue().ToString()) < double.Parse(this.InParameters["valueB"].GetValue().ToString()))
                {
                    return (this.OutParameters["<"].Value as Node).Execute();
                }
            }
            return true;
        }
    }
}
