using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("ConvertCSVToFileNode", "Convert CSV To File", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Convert CSV to file")]
    public class ConvertCSVToFileNode : Node
    {
        public ConvertCSVToFileNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ConvertCSVToFileNode).Name)
        {
            this.InParameters.Add("csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), true));
            this.OutParameters.Add("file", new NodeParameter(this, "file", typeof(System.Byte[]), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if(parameter.Name == "file")
            {
                var csv = this.InParameters["csv"].GetValue() as CreateCSVNode;
                var headers = string.Join(";", csv.Columns) + ";";

                var memoryStream = new MemoryStream();
                TextWriter writer = new StreamWriter(memoryStream);
                writer.WriteLine(headers);
                foreach(var row in csv.Rows)
                {
                    writer.WriteLine(string.Join(";", row.Values));
                }
                writer.Flush();
                memoryStream.Position = 0;
                return memoryStream.ToArray();

            }
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            return true;
        }
    }
}
