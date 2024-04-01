using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("WaitNode", "Wait", NodeTypeEnum.Function, "Time", CustomIcon = "timer")]
    [NodeGraphDescription("Wait a amount of time before executing")]
    public  class WaitNode : Node
    {
        public WaitNode(string id, BlockGraph graph)
           : base(id, graph, typeof(WaitNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "timeInMs", new NodeParameter(this, "timeInMs", typeof(int), true) },

            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            System.Threading.Thread.Sleep(int.Parse(this.InParameters["timeInMs"].GetValue().ToString()));
            return true;

        }
    }
}
