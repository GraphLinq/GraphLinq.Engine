using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Branch
{

    [NodeDefinition("ExecutionTimeIntervalNode", "Execution Time Interval", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Allow execution in a interval rate")]
    public class ExecutionTimeIntervalNode : Node
    {
        public ExecutionTimeIntervalNode(string id, BlockGraph graph)
          : base(id, graph, typeof(ExecutionTimeIntervalNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "intervalInSeconds", new NodeParameter(this, "intervalInSeconds", typeof(int), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
            };
        }

        private double willTickAt = 0;
        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var interval = int.Parse(this.InParameters["intervalInSeconds"].GetValue().ToString()) * 1000;
            if (interval < 1000) interval = 1000;
            if (willTickAt == 0)
            {
                willTickAt = DateTimeOffset.Now.ToUnixTimeSeconds() + interval;
                return true;
            }
            if (willTickAt > DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                return false;
            }
            return true;
        }
    }
}
