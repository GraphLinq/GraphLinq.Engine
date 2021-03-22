using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("OnGraphStartNode", "On Graph Start", NodeTypeEnum.Event, "Common")]
    [NodeGraphDescription("This event is called when the graph start, usefull for initialize variables")]
    public class OnGraphStartNode : Node
    {
        public OnGraphStartNode(string id, BlockGraph graph)
                 : base(id, graph, typeof(OnGraphStartNode).Name)
        {
            this.IsEventNode = true;
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            this.Graph.AddCycle(this);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
