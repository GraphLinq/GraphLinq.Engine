using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Math
{
    [NodeDefinition("MultiplyNode", "Multiply A * B", NodeTypeEnum.Function, "Math")]
    [NodeGraphDescription("Calculate the multiplication of value of A * B (both sent in params) and return it as out parameter.")]
    public class MultiplyNode : Node
    {
        public MultiplyNode(string id, BlockGraph graph)
               : base(id, graph, typeof(MultiplyNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "a", new NodeParameter(this, "a", typeof(object), true) },
                { "b", new NodeParameter(this, "b", typeof(object), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(string), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                var varA = Double.Parse(this.InParameters["a"].GetValue().ToString());
                var varB = Double.Parse(this.InParameters["b"].GetValue().ToString());

                return varA * varB;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
