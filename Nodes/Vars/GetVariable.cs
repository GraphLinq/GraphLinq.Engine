using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Vars
{
    [NodeDefinition("GetVariable", "Get variable", NodeTypeEnum.Function, "Base Variable")]
    [NodeGraphDescription("Return the value of the variable pre computed from a Set variable block")]
    public class GetVariable : Node
    {
        public GetVariable(string id, BlockGraph graph)
         : base(id, graph, typeof(GetVariable).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "variableName", new NodeParameter(this, "variableName", typeof(string), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(object), true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                return this.Graph.MemoryVariables[this.InParameters["variableName"].GetValue().ToString()];
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
