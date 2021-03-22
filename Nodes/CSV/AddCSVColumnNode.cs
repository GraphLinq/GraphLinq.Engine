using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("AddCSVColumnNode", "Add Column to CSV", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Add a new column to a csv")]
    public class AddCSVColumnNode : Node
    {
        public AddCSVColumnNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AddCSVColumnNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), true) },
                { "column", new NodeParameter(this, "column", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "csvOut", new NodeParameter(this, "csvOut", typeof(CreateCSVNode), false, null, "", true) }
            };
        }

        public List<string> Columns = new List<string>();

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;
        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "csvOut")
            {
                return this.InParameters["csv"].GetValue();
            }
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            var csv = this.InParameters["csv"].GetValue() as CreateCSVNode;
            csv.Columns.Add(this.InParameters["column"].GetValue().ToString());
            return true;
        }
    }
}
