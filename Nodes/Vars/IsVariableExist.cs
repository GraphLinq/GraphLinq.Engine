using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Vars
{
    [NodeDefinition("IsVariableExist", "Is Variable Exist", NodeTypeEnum.Condition, "Base Variable")]
    [NodeGraphDescription("Check if a variable exist in the graph memory context")]
    public class IsVariableExist : Node
    {
        public IsVariableExist(string id, BlockGraph graph)
              : base(id, graph, typeof(IsVariableExist).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "variableName", new NodeParameter(this, "variableName", typeof(string), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "true", new NodeParameter(this, "true", typeof(Node), false) },
                { "false", new NodeParameter(this, "false", typeof(Node), false) }
            };
        }
        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.Graph.MemoryVariables.ContainsKey(this.InParameters["variableName"].GetValue().ToString()))
            {
                if (this.OutParameters["true"].Value == null) return true;
                return (this.OutParameters["true"].Value as Node).Execute();
            }
            else
            {
                if (this.OutParameters["false"].Value == null) return true;
                return (this.OutParameters["false"].Value as Node).Execute();
            }
        }
    }
}
