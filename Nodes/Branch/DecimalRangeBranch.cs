using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Branch
{
    [NodeDefinition("DecimalRangeBranch", "Decimal Range Branch", NodeTypeEnum.Condition, "Range Condition")]
    [NodeGraphDescription("Trigger a condition of a node execution based over a range of values (included in a min or max number)")]
    public class DecimalRangeBranch : Node
    {
        public DecimalRangeBranch(string id, BlockGraph graph)
            : base(id, graph, typeof(DecimalRangeBranch).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(double), true) },
                { "rangeMax", new NodeParameter(this, "rangeMax", typeof(double), true) },
                { "rangeMin", new NodeParameter(this, "rangeMin", typeof(double), true) }
            };
            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { ">= RangeMax", new NodeParameter(this, ">= RangeMax", typeof(Node), false) },
                { "<= RangeMin", new NodeParameter(this, "<= RangeMin", typeof(Node), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.OutParameters[">= RangeMax"].Value != null && this.OutParameters[">= RangeMax"].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["value"].GetValue().ToString()) >= double.Parse(this.InParameters["rangeMax"].GetValue().ToString()))
                {
                    return (this.OutParameters[">= RangeMax"].Value as Node).Execute();
                }
            }

            if (this.OutParameters["<= RangeMin"].Value != null && this.OutParameters["<= RangeMin"].Value.ToString() != "")
            {
                if (double.Parse(this.InParameters["value"].GetValue().ToString()) <= double.Parse(this.InParameters["rangeMin"].GetValue().ToString()))
                {
                    return (this.OutParameters["<= RangeMin"].Value as Node).Execute();
                }
            }

            return true;
        }
    }
}
