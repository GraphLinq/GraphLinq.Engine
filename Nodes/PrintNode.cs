using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("PrintNode", "Print", NodeTypeEnum.Function, "Log", CustomIcon = "print")]
    [NodeGraphDescription("Display a message in the console logs")]
    [NodeGasConfiguration("10000000000000")]    public class PrintNode : Node
    {
        public PrintNode(string id, BlockGraph graph)
            : base(id, graph, typeof(PrintNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "message", new NodeParameter(this, "message", typeof(string), true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.InParameters["message"].GetValue().ToString().Trim() == string.Empty) return false;
            this.Graph.AppendLog("info", this.InParameters["message"].GetValue().ToString());
            return true;
        }
    }
}
