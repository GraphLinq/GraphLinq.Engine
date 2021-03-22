using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("ConvertCSVToDataNode", "Convert CSV To Data", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Convert CSV to data parameters")]
    public class ConvertCSVToDataNode : Node
    {
        public ConvertCSVToDataNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ConvertCSVToDataNode).Name)
        {
            this.InParameters.Add("csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), true));
            this.OutParameters.Add("data", new NodeParameter(this, "data", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var csv = this.InParameters["csv"].GetValue() as CreateCSVNode;
            var headers = string.Join(";", csv.Columns) + "\n";
            var rows = string.Join(";\n", csv.Rows.Select(x => string.Join(";", x.Values)));
            this.OutParameters["data"].Value = headers + rows;
            return true;
        }
    }
}
