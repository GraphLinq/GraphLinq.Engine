using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("ConvertLastNodeParametersToCSVRowNode", "Convert Last Node Parameters To CSV Row", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Convert last node out parameters to CSV row")]
    public class ConvertLastNodeParametersToCSVRowNode : Node
    {
        public ConvertLastNodeParametersToCSVRowNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ConvertLastNodeParametersToCSVRowNode).Name)
        {
            this.OutParameters.Add("row", new NodeParameter(this, "row", typeof(CreateCSVRowNode), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if(parameter.Name == "row")
            {
                var row = new CreateCSVRowNode(string.Empty, this.Graph);
                foreach (var param in this.LastExecutionFrom.OutParameters)
                {
                    row.Values.Add(param.Key, param.Value.GetValue().ToString());
                }
                return row;
            }
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            return true;
        }
    }
}
