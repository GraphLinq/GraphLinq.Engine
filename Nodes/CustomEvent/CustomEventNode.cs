using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NodeBlock.Engine.Nodes.CustomEvent
{
    [NodeDefinition("CustomEventNode", "Custom Event", NodeTypeEnum.Event, "Common")]
    [NodeGraphDescription("Listen for custom event from the graph")]
    public class CustomEventNode : Node
    {
        public CustomEventNode(string id, BlockGraph graph)
         : base(id, graph, typeof(CustomEventNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("eventName", new NodeParameter(this, "eventName", typeof(string), true));

            this.OutParameters.Add("eventData", new NodeParameter(this, "eventData", typeof(object), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        private Timer timer { get; set; }

        public override void SetupEvent()
        {
            
        }

        public override void BeginCycle()
        {
            this.Next();
        }

        public void OnTriggerEvent(object data)
        {
            var instanciatedParameters = this.InstanciatedParametersForCycle();
            instanciatedParameters["eventData"].SetValue(data);
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void OnStop()
        {

        }
    }
}
