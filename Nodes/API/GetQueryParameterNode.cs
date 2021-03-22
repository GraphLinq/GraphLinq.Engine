using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("GetQueryParameterNode", "Get Query Parameter", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Get the value of a query parameter from a endpoint request")]
    public class GetQueryParameterNode : Node
    {
        public GetQueryParameterNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetQueryParameterNode).Name)
        {
            this.InParameters.Add("requestContext", new NodeParameter(this, "requestContext", typeof(RequestContext), true));
            this.InParameters.Add("key", new NodeParameter(this, "key", typeof(string), true));

            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(string), false));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                var requestContext = this.InParameters["requestContext"].GetValue() as RequestContext;
                return requestContext.Context.Request.Query[this.InParameters["key"].GetValue().ToString()].ToString();
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
