using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("GetTimestampNode", "Get Timestamp", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Return the current timestamp of the engine localtime")]
    public class GetTimestampNode : Node
    {
        public GetTimestampNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTimestampNode).Name)
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
                return DateTimeOffset.Now.ToUnixTimeSeconds();
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
