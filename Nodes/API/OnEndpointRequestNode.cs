﻿using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("OnEndpointRequestNode", "On Endpoint Request", NodeTypeEnum.Event, "Hosted API")]
    [NodeGraphDescription("This event is called when there is a request on the given endpoint")]
    public class OnEndpointRequestNode : Node
    {
        public OnEndpointRequestNode(string id, BlockGraph graph)
         : base(id, graph, typeof(OnEndpointRequestNode).Name)
        {
            this.IsEventNode = true;
            this.InParameters.Add("endpoint", new NodeParameter(this, "endpoint", typeof(HostedEndpoint), true));

            this.OutParameters.Add("requestContext", new NodeParameter(this, "requestContext", typeof(RequestContext), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            var endpoint = this.InParameters["endpoint"].GetValue() as HostedEndpoint;
            if (endpoint == null) return;
            endpoint.EventsNode = this;
        }

        public void OnRequest(RequestContext requestContext)
        {
            var parameters = this.InstanciatedParametersForCycle();
            parameters["requestContext"].SetValue(requestContext);
            this.Graph.AddCycle(this, parameters, "api");
        }

        public override bool OnExecution()
        {
            var endpoint = this.InParameters["endpoint"].GetValue() as HostedEndpoint;
            if (endpoint == null) return false;
            endpoint.EventsNode = this;

            return false;
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
