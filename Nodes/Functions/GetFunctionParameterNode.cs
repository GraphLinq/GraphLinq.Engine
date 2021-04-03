using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("GetFunctionParameterNode", "Get Function Parameter", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Get a call parameter from the current function context")]
    public class GetFunctionParameterNode : Node
    {
        public GetFunctionParameterNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetFunctionParameterNode).Name)
        {
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(object), false));
        }

        public FunctionContext Context { get; set; }
        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                var context = this.Graph.currentCycle.CurrentFunctionContext;
                var name = this.InParameters["name"].GetValue().ToString();
                return context.CallParameters[name];
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
