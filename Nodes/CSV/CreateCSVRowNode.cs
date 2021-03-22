using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("CreateCSVRowNode", "Create CSV Row", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Create new csv row")]
    public class CreateCSVRowNode : Node
    {
        public CreateCSVRowNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateCSVRowNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "row", new NodeParameter(this, "row", typeof(CreateCSVRowNode), false, null, "", true) }
            };
        }

        public Dictionary<string, string> Values = new Dictionary<string, string>();

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;
        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "row")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            return true;
        }
    }
}
