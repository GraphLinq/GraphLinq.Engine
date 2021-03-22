using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("AddArrayElementNode", "Add Array Element", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("Add a element to the array")]
    public class AddArrayElementNode : Node
    {
        public AddArrayElementNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AddArrayElementNode).Name)
        {
            this.InParameters.Add("array", new NodeParameter(this, "array", typeof(List<object>), true));
            this.InParameters.Add("element", new NodeParameter(this, "element", typeof(object), true));
        }

        public List<object> Array { get; set; }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var array = this.InParameters["array"].GetValue() as List<object>;
            array.Add(this.InParameters["element"].GetValue());
            return true;
        }
    }
}
