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
            this.InParameters.Add("file", new NodeParameter(this, "file", typeof(string), true));
            //this.InParameters.Add("method", new NodeParameter(this, "method", typeof(string), true));

            this.OutParameters.Add("endpoint", new NodeParameter(this, "endpoint", typeof(HostedEndpoint), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public string ContentType = "application/json";

        public override bool OnExecution()
        {
            var hostedAPI = this.InParameters["hostedAPI"].GetValue() as HostedGraphAPI;
            if (hostedAPI == null) return false;
            if(this.InParameters["path"].GetValue().ToString().StartsWith("/web") || this.InParameters["path"].GetValue().ToString().StartsWith("web"))
            {
                this.Graph.AppendLog("debug", "The endpoint can't start with 'web', it will create conflict with public web page system");
                return false;
            }

            // Check if this is a static endpoint
            var fileContent = string.Empty;
            if (this.InParameters.ContainsKey("file"))
            {
                if (this.InParameters["file"].GetValue() != null)
                {
                    fileContent = this.InParameters["file"].GetValue().ToString();
                }
            }

            var endpoint = new HostedEndpoint(hostedAPI, this.InParameters["path"].GetValue().ToString(), ContentType, fileContent);
            hostedAPI.Endpoints.Add(endpoint.Route, endpoint);
            this.Graph.AppendLog("debug", "New endpoint registered " +  endpoint.Route);
            this.OutParameters["endpoint"].SetValue(endpoint);
            return true;
        }
    }
}
