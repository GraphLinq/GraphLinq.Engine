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

        public FunctionContext Context { get; set; }
        public override bool CanExecute => true;
        public override bool CanBeExecuted => false;

        public override bool OnExecution()
        {
            this.Context = new FunctionContext(this);
            this.Graph.currentCycle.CurrentFunctionContext = this.Context;
            return true;
        }
    }
}
