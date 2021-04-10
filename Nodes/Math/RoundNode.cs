
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Math
{
    [NodeDefinition("RoundNode", "Round", NodeTypeEnum.Function, "Math")]
    [NodeGraphDescription("Round the value")]
    public class RoundNode : Node
    {
        public RoundNode(string id, BlockGraph graph)
                  : base(id, graph, typeof(RoundNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "number", new NodeParameter(this, "number", typeof(double), true) },
                { "decimal", new NodeParameter(this, "decimal", typeof(int), true) }
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
                var dec = int.Parse(this.InParameters["decimal"].GetValue().ToString());

                return global::System.Math.Round(number, dec);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
