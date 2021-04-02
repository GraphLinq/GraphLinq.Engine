using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Math
{
    [NodeDefinition("FloorNode", "Floor", NodeTypeEnum.Function, "Math")]
    [NodeGraphDescription("Floor the value")]
    public class FloorNode : Node
    {
        public FloorNode(string id, BlockGraph graph)
                  : base(id, graph, typeof(FloorNode).Name)
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

                return global::System.Math.Floor(number);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
