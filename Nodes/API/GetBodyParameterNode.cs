using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API
{
    [NodeDefinition("GetBodyParameterNode", "Get Body Parameter", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Get a value from body parameters")]
    public class GetBodyParameterNode : Node
    {
        public GetBodyParameterNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetBodyParameterNode).Name)
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
                try
                {
                    var requestContext = this.InParameters["requestContext"].GetValue() as RequestContext;
                    var key = this.InParameters["key"].GetValue().ToString();
                    return requestContext.RequestBodyObject[key].ToString();
                }
                catch
                {
                    this.Graph.AppendLog("error", "Error when fetching the body parameter, maybe the parameter was not given");
                    return string.Empty;
                }
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
