using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("GetTimestampMsOffsetNode", "Get Milliseconds Timestamp Offset", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Return offset timestamp with milliseconds of the engine localtime")]
    public class GetTimestampMsOffsetNode : Node
    {
        public GetTimestampMsOffsetNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTimestampMsOffsetNode).Name)
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
                    if (period == "w") { duration = duration * 7; }

                    switch (period)
                    {
                        case "h": return DateTimeOffset.Now.AddHours(-duration).ToUnixTimeMilliseconds();
                        case "d": return DateTimeOffset.Now.AddDays(-duration).ToUnixTimeMilliseconds();
                        case "w": return DateTimeOffset.Now.AddDays(-duration).ToUnixTimeMilliseconds();
                        case "m": return DateTimeOffset.Now.AddMonths(-duration).ToUnixTimeMilliseconds();
                        case "y": return DateTimeOffset.Now.AddYears(-duration).ToUnixTimeMilliseconds();
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
