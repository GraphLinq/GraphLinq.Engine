using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("SetCustomResponseTimeoutNode", "Set Custom Response Timeout", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Set a custom timeout for a endpoint response")]
    public class SetCustomResponseTimeoutNode : Node
    {
        public SetCustomResponseTimeoutNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SetCustomResponseTimeoutNode).Name)
        {
            this.InParameters.Add("endpoint", new NodeParameter(this, "endpoint", typeof(object), true));
            this.InParameters.Add("timeInMs", new NodeParameter(this, "timeInMs", typeof(int), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var endpoint = this.InParameters["endpoint"].GetValue() as HostedEndpoint;
            if (endpoint == null) return false;
            var ttl = int.Parse(this.InParameters["timeInMs"].GetValue().ToString());
            if (ttl > 60000)
            {
                this.Graph.AppendLog("error", "The timeout need to be =< 60000");
                return false;
            }
            endpoint.CustomTimeout = ttl;
            return true;
        }
    }
}
