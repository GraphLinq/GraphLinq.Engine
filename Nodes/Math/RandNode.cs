using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Math
{
    [NodeDefinition("RandNode", "Random Number", NodeTypeEnum.Function, "Math")]
    [NodeGraphDescription("Get a random number")]
    public class RandNode : Node
    {
        public RandNode(string id, BlockGraph graph)
               : base(id, graph, typeof(RandNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "min", new NodeParameter(this, "min", typeof(int), true) },
                { "max", new NodeParameter(this, "max", typeof(int), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "number", new NodeParameter(this, "number", typeof(int), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "number")
            {
                var min = int.Parse(this.InParameters["min"].GetValue().ToString());
                var max = int.Parse(this.InParameters["max"].GetValue().ToString());
                var rand = new Random();

                return rand.Next(min, max);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
