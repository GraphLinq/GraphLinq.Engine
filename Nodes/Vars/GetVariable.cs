using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Vars
{
    [NodeDefinition("GetVariable", "Get variable", NodeTypeEnum.Variable, "Base Variable")]
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
                { "value", new NodeParameter(this, "value", typeof(object), true) },
                { "directName", new NodeParameter(this, "directName", typeof(string), true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                if(this.InParameters["variableName"].GetValue() == null)
                {
                    return this.Graph.MemoryVariables[this.OutParameters["directName"].GetValue().ToString()];
                }
                else
                {
                    return this.Graph.MemoryVariables[this.InParameters["variableName"].GetValue().ToString()];
                }
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
