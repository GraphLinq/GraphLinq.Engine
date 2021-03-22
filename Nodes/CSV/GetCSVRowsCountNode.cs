using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("GetCSVRowsCountNode", "Get CSV Rows Count", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Get CSV Rows Count")]
    public class GetCSVRowsCountNode : Node
    {
        public GetCSVRowsCountNode(string id, BlockGraph graph)
             : base(id, graph, typeof(GetCSVRowsCountNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "count", new NodeParameter(this, "count", typeof(int), false) },
            };
        }
        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if(parameter.Name == "count")
            {
                return (this.InParameters["csv"].GetValue() as CreateCSVNode).Rows.Count;
            }
            return base.ComputeParameterValue(parameter, value);
        }

        public override bool OnExecution()
        {
            return true;
        }
    }
}
