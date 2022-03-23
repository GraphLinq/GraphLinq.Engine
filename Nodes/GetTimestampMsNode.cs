using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("GetTimestampMsNode", "Get Milliseconds Timestamp", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Return the current milliseconds timestamp of the engine localtime")]
    public class GetTimestampMsNode : Node
    {
        public GetTimestampMsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTimestampMsNode).Name)
        {
            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "timestamp", new NodeParameter(this, "timestamp", typeof(long), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "timestamp")
            {
                return DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
