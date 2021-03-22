using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("SetRequestResponseBodyNode", "Set Request Response Body", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Set the body for a request from a endpoint call")]
    public class SetRequestResponseBodyNode : Node
    {
        public SetRequestResponseBodyNode(string id, BlockGraph graph)
       : base(id, graph, typeof(SetRequestResponseBodyNode).Name)
        {
            this.InParameters.Add("requestContext", new NodeParameter(this, "requestContext", typeof(RequestContext), true));
            this.InParameters.Add("body", new NodeParameter(this, "body", typeof(string), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var requestContext = this.InParameters["requestContext"].GetValue() as RequestContext;
            requestContext.Body = this.InParameters["body"].GetValue().ToString();
            return true;
        }
    }
}
