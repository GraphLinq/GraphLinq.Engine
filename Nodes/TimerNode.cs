using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Timers;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("TimerNode", "Timer", NodeTypeEnum.Event, "Time")]
    [NodeGasConfiguration("10000000000000")]
    [NodeGraphDescription("Start a timer that will init a new execution cycle, from in parameter specified time.")]
    public class TimerNode : Node
    {
        public TimerNode(string id, BlockGraph graph)
          : base(id, graph, typeof(TimerNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("intervalInSeconds", new NodeParameter(this, "intervalInSeconds", typeof(int), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        private Timer timer { get; set; }

        public override void SetupEvent()
        {
            var time = int.Parse(this.InParameters["intervalInSeconds"].GetValue().ToString());
            if (time <= 0)
            {
                time = 1;
            }
            timer = new Timer(time * 1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            this.Graph.AddCycle(this);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Graph.AddCycle(this);
        }

        public override void BeginCycle()
        {
            this.Next();
        }

        public override void OnStop()
        {
            if(this.timer != null) this.timer.Stop();
        }
    }
}
