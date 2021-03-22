using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("CompleteRequestContextNode", "Complete Request Context", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Send the response for the request context of a endpoint call")]
    public class CompleteRequestContextNode : Node
    {
        public CompleteRequestContextNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CompleteRequestContextNode).Name)
        {
            this.InParameters.Add("requestContext", new NodeParameter(this, "requestContext", typeof(RequestContext), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var requestContext = this.InParameters["requestContext"].GetValue() as RequestContext;
            requestContext.Complete(true);
            return true;
        }
    }
}
