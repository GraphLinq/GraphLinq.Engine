using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("BoolNode", "Boolean", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("In computer science, the Boolean data type is a data type that has one of two possible values (usually denoted true and false) which is intended to represent the two truth values of logic and Boolean algebra")]
    public class BoolNode : Node
    {
        public BoolNode(string id, BlockGraph graph)
            : base(id, graph, typeof(BoolNode).Name)
        {
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(bool), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
