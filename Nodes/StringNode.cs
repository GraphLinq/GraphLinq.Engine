using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{

    [NodeDefinition("StringNode", "String", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("A string is a data type used in programming, such as an integer and floating point unit, but is used to represent text rather than numbers. It is comprised of a set of characters that can also contain spaces and numbers.")]
    public class StringNode : Node
    {
        public StringNode(string id, BlockGraph graph)
          : base(id, graph, typeof(StringNode).Name)
        {
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(string), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
