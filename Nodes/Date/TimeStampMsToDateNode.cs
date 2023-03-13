using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Date
{
    [NodeDefinition("TimeStampMsToDateNode", "Millisecond Timestamp to Date", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Convert a Timestamp with Milliseconds to Date")]
    public class TimeStampMsToDateNode : Node
    {
        public TimeStampMsToDateNode(string id, BlockGraph graph)
            : base(id, graph, typeof(TimeStampMsToDateNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "timestamp", new NodeParameter(this, "timestamp", typeof(long), true) }
            };

            this.OutParameters.Add("date", new NodeParameter(this, "date", typeof(object), false));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "date")
            {
                var timestamp = long.Parse(this.InParameters["timestamp"].GetValue().ToString());
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(timestamp).ToLocalTime();
                return dtDateTime;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
