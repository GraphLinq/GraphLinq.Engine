using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("ClearArrayNode", "Clear Array", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("Clear all elements in an array")]
    public class ClearArrayNode : Node
    {
        public ClearArrayNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ClearArrayNode).Name)
        {
            this.InParameters.Add("array", new NodeParameter(this, "array", typeof(List<object>), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var array = this.InParameters["array"].GetValue() as List<object>;
            array.Clear();
            return true;
        }
    }
}
