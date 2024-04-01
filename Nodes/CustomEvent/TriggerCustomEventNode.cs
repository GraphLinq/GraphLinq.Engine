using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Nodes.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Engine.Nodes.CustomEvent
{
    [NodeDefinition("TriggerCustomEventNode", "Trigger Custom Event", NodeTypeEnum.Function, "Common")]
    [NodeGraphDescription("Trigger a custom event")]
    public class TriggerCustomEventNode : Node
    {
        public TriggerCustomEventNode(string id, BlockGraph graph)
            : base(id, graph, typeof(TriggerCustomEventNode).Name)
        {
            this.InParameters.Add("eventName", new NodeParameter(this, "eventName", typeof(string), true));
            this.InParameters.Add("data", new NodeParameter(this, "data", typeof(object), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var customEventNode = this.Graph.Nodes.FirstOrDefault(x => x.Value.NodeType == "CustomEventNode" &&
                x.Value.InParameters["eventName"].GetValue().ToString() == this.InParameters["eventName"].GetValue().ToString()).Value as CustomEventNode;
            if (customEventNode == null)
            {
                this.Graph.AppendLog("warn", "Custom event " + this.InParameters["eventName"].GetValue().ToString() + " doesnt exist in the graph");
                return false;
            }

            customEventNode.OnTriggerEvent(this.InParameters["data"].GetValue());

            return true;
        }
    }
}
