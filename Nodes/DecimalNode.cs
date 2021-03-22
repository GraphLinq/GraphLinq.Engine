using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("DecimalNode", "Decimal", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("The decimal data type is an exact numeric data type defined by its precision (total number of digits) and scale (number of digits to the right of the decimal point). ... Scale can be 0 (no digits to the right of the decimal point).")]
    public class DecimalNode : Node
    {
        public DecimalNode(string id, BlockGraph graph)
            : base(id, graph, typeof(DecimalNode).Name)
        {
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(double), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
