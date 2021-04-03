using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("CallFunctionNode", "Call Function", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Call a function in the graph")]
    public class CallFunctionNode : Node
    {
        public CallFunctionNode(string id, BlockGraph graph)
        : base(id, graph, typeof(CallFunctionNode).Name)
        {
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("parameters", new NodeParameter(this, "parameters", typeof(Dictionary<string, object>), true));

            this.OutParameters.Add("results", new NodeParameter(this, "results", typeof(Dictionary<string, object>), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var functions = this.Graph.Nodes.ToList().FindAll(x => x.Value.NodeType == "FunctionNode");
            var functionNode = this.Graph.Nodes.FirstOrDefault(x => x.Value.NodeType == "FunctionNode" &&
                x.Value.InParameters["name"].GetValue().ToString() == this.InParameters["name"].GetValue().ToString()).Value as FunctionNode;
            if(functionNode == null)
            {
                this.Graph.AppendLog("error", "Function " + this.InParameters["name"].GetValue().ToString() + " doesnt exist in the graph");
                return false;
            }
            functionNode.CallParameters = this.InParameters["parameters"].GetValue() as Dictionary<string, object>;
            functionNode.Execute();
            this.OutParameters["results"].SetValue(functionNode.Context.ReturnValues);
            return true;
        }
    }
}
