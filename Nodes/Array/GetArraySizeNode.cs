using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("GetArraySizeNode", "Get Array Size", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("Get the size of an array")]
    public class GetArraySizeNode : Node
    {
        public GetArraySizeNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetArraySizeNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "array", new NodeParameter(this, "array", typeof(List<object>), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "size", new NodeParameter(this, "size", typeof(int), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "size")
            {
                var array = this.InParameters["array"].GetValue() as List<object>;
                return array.Count;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
