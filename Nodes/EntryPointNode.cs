using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("EntryPointNode", "Entry Point", NodeTypeEnum.EntryPoint, "Common")]
    [NodeGraphDescription("Basic Graph entry point, start the execution of a graph")]
    public class EntryPointNode : Node
    {
        public EntryPointNode(string id, BlockGraph graph)
            : base(id, graph, typeof(EntryPointNode).Name)
        {

        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => false;

        public override bool OnExecution()
        {

            return true;
        }
    }
}
