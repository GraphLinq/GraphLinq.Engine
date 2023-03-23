using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("FunctionNode", "Function", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Create a new function")]
    public class FunctionNode : Node
    {
        public FunctionNode(string id, BlockGraph graph)
          : base(id, graph, typeof(FunctionNode).Name)
        {
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
        }

        public Dictionary<string, object> CallParameters { get; set; }
        public FunctionContext Context { get; set; }
        public override bool CanExecute => true;
        public override bool CanBeExecuted => false;

        public override bool OnExecution()
        {
            this.Context = new FunctionContext(this);
            this.Graph.currentCycle.CurrentFunctionContext = this.Context;
            return true;
        }

        public List<string> GetFunctionInParameters()
        {
            var parameters = new List<string>();

            this.dissectRequiredParameters(this, parameters);

            return parameters;
        }

        private void dissectRequiredParameters(Node fromNode, List<string> parameters)
        {
            if (fromNode.OutNode == null) return;
            foreach (var inParametersNode in fromNode.OutNode.InParameters)
            {
                if(inParametersNode.Value.GetNode() != null)
                {
                    if (typeof(GetFunctionParameterNode) == inParametersNode.Value.GetNode().GetType())
                    {
                        var paramNode = inParametersNode.Value.GetNode() as GetFunctionParameterNode;
                        parameters.Add(paramNode.InParameters["name"].GetValue().ToString());
                    }
                }
            }
            
            this.dissectRequiredParameters(fromNode.OutNode, parameters);
        }
    }
}
