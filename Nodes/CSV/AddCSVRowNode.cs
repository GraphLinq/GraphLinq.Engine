using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("AddCSVRowNode", "Add CSV Row", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Add a row to a csv")]
    public class AddCSVRowNode : Node
    {
        public AddCSVRowNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AddCSVRowNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), true) },
                { "row", new NodeParameter(this, "row", typeof(CreateCSVRowNode), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            var csv = this.InParameters["csv"].GetValue() as CreateCSVNode;
            var row = this.InParameters["row"].GetValue() as CreateCSVRowNode;
            foreach(var value in row.Values)
            {
                if(!csv.Columns.Contains(value.Key))
                {
                    csv.Columns.Add(value.Key);
                }
            }
            csv.Rows.Add(row.Values);
            row.Values = new Dictionary<string, string>();
            return true;
        }
    }
}
