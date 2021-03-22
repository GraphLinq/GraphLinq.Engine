using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Vars
{
    [NodeDefinition("VariablePortalNode", "Variable Portal", NodeTypeEnum.Function, "Base Variable")]
    [NodeGraphDescription("Pass a variable from in to the out, it's used or organize your graph")]
    public class VariablePortalNode : Node
    {
        public VariablePortalNode(string id, BlockGraph graph)
         : base(id, graph, typeof(VariablePortalNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "in", new NodeParameter(this, "in", typeof(object), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "out", new NodeParameter(this, "out", typeof(object), true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "out")
            {
                return this.InParameters["in"].GetValue();
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
