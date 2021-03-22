using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Vars
{
    [NodeDefinition("SetVariable", "Set variable", NodeTypeEnum.Function, "Base Variable")]
    [NodeGraphDescription("Set a variable value (can be any type) in the graph memory context")]
    public class SetVariable : Node
    {
        public SetVariable(string id, BlockGraph graph)
            : base(id, graph, typeof(SetVariable).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "variableName", new NodeParameter(this, "variableName", typeof(string), true) },
                { "value", new NodeParameter(this, "value", typeof(object), true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var key = (string)this.InParameters["variableName"].GetValue();
            if(!this.Graph.MemoryVariables.ContainsKey(key)) {
                this.Graph.MemoryVariables.Add(key, this.InParameters["value"].GetValue());
            }
            else
            {
                this.Graph.MemoryVariables[key] = this.InParameters["value"].GetValue();
            }
            return true;
        }
    }
}
