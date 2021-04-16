using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("EndpointCacheResponseNode", "Add Endpoint Cache", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Add a cache for the response of the endpoint")]
    public class EndpointCacheResponseNode : Node
    {
        public EndpointCacheResponseNode(string id, BlockGraph graph)
            : base(id, graph, typeof(EndpointCacheResponseNode).Name)
        {
            this.InParameters.Add("endpoint", new NodeParameter(this, "endpoint", typeof(object), true));
            this.InParameters.Add("ttl", new NodeParameter(this, "ttl", typeof(int), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var endpoint = this.InParameters["endpoint"].GetValue() as HostedEndpoint;
            if (endpoint == null) return false;
            var ttl = int.Parse(this.InParameters["ttl"].GetValue().ToString());
            if(ttl < 10)
            {
                this.Graph.AppendLog("error", "The cache TTL need to be >= 10");
                return false;
            }
            endpoint.CacheTTL = ttl;
            return true;
        }
    }
}
