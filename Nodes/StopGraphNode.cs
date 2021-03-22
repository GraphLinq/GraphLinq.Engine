using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("StopGraphNode", "Stop Graph", NodeTypeEnum.Function, "Common")]
    [NodeGraphDescription("Stop the execution of the current graph")]
    [NodeGasConfiguration("0")]
    public class StopGraphNode : Node
    {
        public StopGraphNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StopGraphNode).Name)
        {
   
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            this.Graph.Stop();
            return true;
        }
    }
}
