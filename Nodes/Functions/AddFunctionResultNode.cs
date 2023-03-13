using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    [NodeDefinition("AddFunctionResultNode", "Set Function Result", NodeTypeEnum.Function, "Function")]
    [NodeGraphDescription("Set a value returned by the function")]
    public class AddFunctionResultNode : Node
    {
        public AddFunctionResultNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AddFunctionResultNode).Name)
        {
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("value", new NodeParameter(this, "value", typeof(object), true));
        }

        public FunctionContext Context { get; set; }
        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var context = this.Graph.currentCycle.CurrentFunctionContext;
            var name = this.InParameters["name"].GetValue().ToString();
            var value = this.InParameters["value"].GetValue();
            if(context.ReturnValues.ContainsKey(name))
            {
                context.ReturnValues[name] = value;
            }
            else
            {
                context.ReturnValues.Add(name, value);
            }
            return true;
        }
    }
}
