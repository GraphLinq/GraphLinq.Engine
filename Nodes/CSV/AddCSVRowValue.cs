using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("AddCSVRowValue", "Add CSV Row Value", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Add a value to a csv row")]
    public class AddCSVRowValue : Node
    {
        public AddCSVRowValue(string id, BlockGraph graph)
         : base(id, graph, typeof(AddCSVRowValue).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "row", new NodeParameter(this, "row", typeof(CreateCSVRowNode), true) },
                { "name", new NodeParameter(this, "name", typeof(string), true) },
                { "value", new NodeParameter(this, "value", typeof(string), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "rowOut", new NodeParameter(this, "rowOut", typeof(CreateCSVRowNode), false, null, "", true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;
        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "rowOut")
            {
                return this.InParameters["row"].GetValue();
            }
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            var row = this.InParameters["row"].GetValue() as CreateCSVRowNode;
            row.Values.Add(this.InParameters["name"].GetValue().ToString(), this.InParameters["value"].GetValue().ToString());
            return true;
        }
    }
}
