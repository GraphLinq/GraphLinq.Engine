using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Data
{
    [NodeDefinition("CombineDataNode", "Combine Data", NodeTypeEnum.Function, "Data")]
    [NodeGraphDescription("Combine two data in single one")]
    public class CombineDataNode : Node
    {
        public CombineDataNode(string id, BlockGraph graph)
             : base(id, graph, typeof(CombineDataNode).Name)
        {
            this.InParameters.Add("data1", new NodeParameter(this, "data1", typeof(object), true));
            this.InParameters.Add("data2", new NodeParameter(this, "data2", typeof(object), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "combinedData", new NodeParameter(this, "combinedData", typeof(object), false, null, "", true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "combinedData")
            {
                var combinedData = new List<Encoding.Models.KeyValueData>();
                var data1 = this.InParameters["data1"].GetValue() as List<Encoding.Models.KeyValueData>;
                var data2 = this.InParameters["data2"].GetValue() as List<Encoding.Models.KeyValueData>;
                combinedData.AddRange(data1);
                combinedData.AddRange(data2);
                return combinedData;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
