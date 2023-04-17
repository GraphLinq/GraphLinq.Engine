using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Common
{
    [NodeDefinition("GetGraphRunningSinceNode", "Get Graph Running Since Time", NodeTypeEnum.Function, "Common")]
    [NodeGraphDescription("Get the time since the graph has been started")]
    public class GetGraphRunningSinceNode : Node
    {
        public GetGraphRunningSinceNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetGraphRunningSinceNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>();

            this.OutParameters.Add("time", new NodeParameter(this, "time", typeof(string), false));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "time")
            {
                return ConvertToHumanText(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - this.Graph.StartedAt);
            }
            return base.ComputeParameterValue(parameter, value);
        }

        public string ConvertToHumanText(long unixTimestampSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(unixTimestampSeconds);

            string result = "";

            if (timeSpan.Days > 0)
            {
                result += timeSpan.Days + "d ";
            }

            if (timeSpan.Hours > 0)
            {
                result += timeSpan.Hours + "h ";
            }

            if (timeSpan.Minutes > 0)
            {
                result += timeSpan.Minutes + "m ";
            }

            result += timeSpan.Seconds + "s";

            return result.TrimEnd();
        }
    }
}
