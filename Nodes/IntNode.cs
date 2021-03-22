using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("IntNode", "Integer", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("An integer format is a data type in computer programming. ... Integers represent whole units. Integers occupy less space in memory, but this space-saving feature limits the magnitude of the integer that can be stored.")]
    public class IntNode : Node
    {
        public IntNode(string id, BlockGraph graph)
            : base(id, graph, typeof(IntNode).Name)
        {
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(int), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
