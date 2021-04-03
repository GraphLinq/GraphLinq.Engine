using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("GetFunctionResultParameterNode", "Get Function Result Parameter", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Get a result parameter from a function result")]
    public class GetFunctionResultParameterNode : Node
    {
        public GetFunctionResultParameterNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetFunctionResultParameterNode).Name)
        {
            this.InParameters.Add("results", new NodeParameter(this, "results", typeof(Dictionary<string, object>), true));
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));

            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(object), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var name = this.InParameters["name"].GetValue().ToString();
            var results = this.InParameters["results"].GetValue() as Dictionary<string, object>;
            if(!results.ContainsKey(name))
            {
                this.Graph.AppendLog("error", "No result named " + name + " in function results");
                return false;
            }
            this.OutParameters["value"].SetValue(results[name]);
            return true;
        }
    }
}
