using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NodeBlock.Engine.Nodes.Math
{
    [NodeDefinition("BounceRateIntervalNode", "Bounce Rate Interval", NodeTypeEnum.Function, "Math")]
    [NodeGraphDescription("Calculate the bounce rate in the given interval, you can use the Percentage value only when the OnIntervalTick trigger")]
    public class BounceRateIntervalNode : Node
    {
        public BounceRateIntervalNode(string id, BlockGraph graph)
          : base(id, graph, typeof(BounceRateIntervalNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(object), true) },
                { "interval", new NodeParameter(this, "interval", typeof(int), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "percentage", new NodeParameter(this, "percentage", typeof(double), false, null, "", true) },
                { "onIntervalTick", new NodeParameter(this, "onIntervalTick", typeof(Node), false) },
            };
        }

        private double value;
        private double willTickAt = 0;

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var interval = int.Parse(this.InParameters["interval"].GetValue().ToString());
            if(willTickAt == 0) // Set the first value
            {
                value = double.Parse(this.InParameters["value"].GetValue().ToString(), CultureInfo.InvariantCulture);
                willTickAt = DateTimeOffset.Now.ToUnixTimeSeconds() + interval;
            }
            if(willTickAt <= DateTimeOffset.Now.ToUnixTimeSeconds()) // When the tick at is trigger, call the event
            {
                var value2 = double.Parse(this.InParameters["value"].GetValue().ToString(), CultureInfo.InvariantCulture);
                var bounceRate = ((value2 - value) / global::System.Math.Abs(value)) * 100;

                value = value2;
                willTickAt = DateTimeOffset.Now.ToUnixTimeSeconds() + interval;
                this.OutParameters["percentage"].SetValue(bounceRate);

                (this.OutParameters["onIntervalTick"].Value as Node).Execute();
            }
            return true;
        }
    }
}
