using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Math
{
    [NodeDefinition("CeilNode", "Ceiling", NodeTypeEnum.Function, "Math")]
    [NodeGraphDescription("Ceiling the value")]
    public class CeilNode : Node
    {
        public CeilNode(string id, BlockGraph graph)
                  : base(id, graph, typeof(CeilNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "number", new NodeParameter(this, "number", typeof(double), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(double), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                var number = Double.Parse(this.InParameters["number"].GetValue().ToString());

                return global::System.Math.Ceiling(number);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
