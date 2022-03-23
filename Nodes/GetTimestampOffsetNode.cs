using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("GetTimestampOffsetNode", "Get Timestamp Offset", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Return offset timestamp of the engine localtime")]
    public class GetTimestampOffsetNode : Node
    {
        public GetTimestampOffsetNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTimestampOffsetNode).Name)
        {

            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "offset", new NodeParameter(this, "offset", typeof(string), true) }
            };

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
                string offset = this.InParameters["offset"].GetValue().ToString();
                string durats = string.Empty;
                int duration = 0;
                string period = string.Empty;

                for (int i = 0; i < offset.Length; i++)
                {
                    if (Char.IsDigit(offset[i]))
                    {
                        durats += offset[i];
                    }
                    else if (Char.IsLetter(offset[i]))
                    {
                        period += offset[i];
                    }
                    else
                    {
                        //
                    }
                }

                if (durats.Length > 0)
                {
                    duration = int.Parse(durats);
                }
                else
                {
                    return false;
                }

                if (period.Length > 0)
                {
                    switch (period)
                    {
                        case "h": return DateTimeOffset.Now.AddHours(-duration).ToUnixTimeSeconds();
                        case "d": return DateTimeOffset.Now.AddDays(-duration).ToUnixTimeSeconds();
                        case "m": return DateTimeOffset.Now.AddMonths(-duration).ToUnixTimeSeconds();
                        case "y": return DateTimeOffset.Now.AddYears(-duration).ToUnixTimeSeconds();
                        default: return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
