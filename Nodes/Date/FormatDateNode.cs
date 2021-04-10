using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Date
{
    [NodeDefinition("FormatDateNode", "Format Date", NodeTypeEnum.Function, "Time")]
    [NodeGraphDescription("Format a date with a given pattern")]
    public class FormatDateNode : Node
    {
        public FormatDateNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FormatDateNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "date", new NodeParameter(this, "date", typeof(object), true) },
                { "format", new NodeParameter(this, "format", typeof(string), true) }
            };

            this.OutParameters.Add("dateString", new NodeParameter(this, "dateString", typeof(string), false));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "dateString")
            {
                var date = this.InParameters["date"].GetValue() as DateTime?;
                return date.Value.ToString(this.InParameters["format"].GetValue().ToString());
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
