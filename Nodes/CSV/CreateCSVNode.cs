using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("CreateCSVNode", "Create CSV", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Create a new CSV instance")]
    public class CreateCSVNode : Node
    {
        public CreateCSVNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateCSVNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "name", new NodeParameter(this, "name", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), false, null, "", true) }
            };
        }

        public List<string> Columns = new List<string>();
        public List<Dictionary<string, string>> Rows = new List<Dictionary<string, string>>();

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;
        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "csv")
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
