using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("ExposeAPIBlockNode", "Expose API", NodeTypeEnum.Connector, "Hosted API")]
    [NodeGraphDescription("Expose a HTTP API for the graph, this will generate a random port and a random id in output")]
    public class ExposeAPIBlockNode : Node
    {
        public ExposeAPIBlockNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ExposeAPIBlockNode).Name)
        {
            this.CanBeSerialized = false;
            this.OutParameters.Add("hostedAPI", new NodeParameter(this, "hostedAPI", typeof(HostedGraphAPI), true));
            this.OutParameters.Add("url", new NodeParameter(this, "url", typeof(string), true));
        }

        public HostedGraphAPI HostedAPI { get; set; }

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.HostedAPI = new HostedGraphAPI(this.Graph);
            this.Next();
        }

        public override void OnStop()
        {
            //TODO: Stop API
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "hostedAPI")
            {
                return this.HostedAPI;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
