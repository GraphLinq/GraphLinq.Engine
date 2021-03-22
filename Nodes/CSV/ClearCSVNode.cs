using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.CSV
{
    [NodeDefinition("ClearCSVNode", "Clear CSV Rows", NodeTypeEnum.Function, "CSV")]
    [NodeGraphDescription("Clear CSV Rows")]
    public class ClearCSVNode : Node
    {
        public ClearCSVNode(string id, BlockGraph graph)
             : base(id, graph, typeof(ClearCSVNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "csv", new NodeParameter(this, "csv", typeof(CreateCSVNode), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
            };
        }
        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            (this.InParameters["csv"].GetValue() as CreateCSVNode).Rows.Clear();
            return true;
        }
    }
}
