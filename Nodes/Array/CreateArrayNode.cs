using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("CreateArrayNode", "Create Array", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("An array can store multiple variable in it")]

    public class CreateArrayNode : Node
    {
        public CreateArrayNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateArrayNode).Name)
        {
            this.OutParameters.Add("array", new NodeParameter(this, "array", typeof(List<object>), true));
        }

        public List<object> Array { get; set; }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            this.Array = new List<object>();
            this.OutParameters["array"].SetValue(this.Array);
            return true;
        }
    }
}
