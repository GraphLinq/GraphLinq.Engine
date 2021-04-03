using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("CreateFunctionParametersNode", "Create Function Parameters", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Create a empty array for function parameters")]
    public class CreateFunctionParametersNode : Node
    {
        public CreateFunctionParametersNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateFunctionParametersNode).Name)
        {
            this.OutParameters.Add("parameters", new NodeParameter(this, "parameters", typeof(Dictionary<string, object>), false));
        }

        public Dictionary<string, object> FunctionParameters { get; set; }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            this.FunctionParameters = new Dictionary<string, object>();
            this.OutParameters["parameters"].SetValue(FunctionParameters);
            return true;
        }
    }
}
