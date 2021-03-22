using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("AddAPIEndpointNode", "Add API Endpoint", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Add new endpoint to a Hosted API")]
    public class AddAPIEndpointNode : Node
    {
        public AddAPIEndpointNode(string id, BlockGraph graph)
        : base(id, graph, typeof(AddAPIEndpointNode).Name)
        {
            this.InParameters.Add("hostedAPI", new NodeParameter(this, "hostedAPI", typeof(HostedGraphAPI), true));
            this.InParameters.Add("path", new NodeParameter(this, "path", typeof(string), true));
            //this.InParameters.Add("method", new NodeParameter(this, "method", typeof(string), true));

            this.OutParameters.Add("endpoint", new NodeParameter(this, "endpoint", typeof(HostedEndpoint), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var hostedAPI = this.InParameters["hostedAPI"].GetValue() as HostedGraphAPI;
            if (hostedAPI == null) return false;
            var endpoint = new HostedEndpoint(hostedAPI, this.InParameters["path"].GetValue().ToString());
            hostedAPI.Endpoints.Add(endpoint.Route, endpoint);
            this.Graph.AppendLog("debug", "New endpoint registered " +  endpoint.Route);
            this.OutParameters["endpoint"].SetValue(endpoint);
            return true;
        }
    }
}
