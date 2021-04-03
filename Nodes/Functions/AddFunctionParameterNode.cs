using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("AddFunctionParameterNode", "Add Function Parameter", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Add a new parameter to a function parameters array")]
    public class AddFunctionParameterNode : Node
    {
        public AddFunctionParameterNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AddFunctionParameterNode).Name)
        {
            this.InParameters.Add("parameters", new NodeParameter(this, "parameters", typeof(Dictionary<string, object>), true));
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("value", new NodeParameter(this, "value", typeof(object), true));

            this.OutParameters.Add("outParameters", new NodeParameter(this, "outParameters", typeof(Dictionary<string, object>), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var parameters = this.InParameters["parameters"].GetValue() as Dictionary<string, object>;
            var name = this.InParameters["name"].GetValue().ToString();
            var value = this.InParameters["value"].GetValue();

            if(parameters.ContainsKey(name))
            {
                parameters[name] = value;
            }
            else
            {
                parameters.Add(name, value);
            }
            this.OutParameters["outParameters"].SetValue(parameters);

            return true;
        }
    }
}
