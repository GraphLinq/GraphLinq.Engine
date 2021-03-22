using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("LongNode", "Long", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("The long data type is a 64-bit two's complement integer. The signed long has a minimum value of -263 and a maximum value of 263-1.")]
    public class LongNode : Node
    {
        public LongNode(string id, BlockGraph graph)
            : base(id, graph, typeof(LongNode).Name)
        {
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(long), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
