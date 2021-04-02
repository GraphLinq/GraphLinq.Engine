using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("GetArrayElementAtIndexNode", "Get Array Element At Index", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("Get a element from a array at a specific index")]
    public class GetArrayElementAtIndexNode : Node
    {
        public GetArrayElementAtIndexNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetArrayElementAtIndexNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "array", new NodeParameter(this, "array", typeof(List<object>), true) },
                { "index", new NodeParameter(this, "index", typeof(int), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "element", new NodeParameter(this, "element", typeof(object), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "element")
            {
                var array = this.InParameters["array"].GetValue() as List<object>;
                return array[int.Parse(this.InParameters["index"].GetValue().ToString())];
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
