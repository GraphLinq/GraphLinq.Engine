using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("KeepArrayLastElementsNode", "Keep last X elements Array", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("keep last X elements in a array")]
    public class KeepArrayLastElementsNode : Node
    {
        public KeepArrayLastElementsNode(string id, BlockGraph graph)
         : base(id, graph, typeof(KeepArrayLastElementsNode).Name)
        {
            this.InParameters.Add("array", new NodeParameter(this, "array", typeof(List<object>), true));
            this.InParameters.Add("amountToKeep", new NodeParameter(this, "amountToKeep", typeof(int), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var array = this.InParameters["array"].GetValue() as List<object>;
            var amountToKeep = int.Parse(this.InParameters["amountToKeep"].GetValue().ToString());
            if (array.Count <= amountToKeep) return true;
            var tempArray = array.TakeLast(amountToKeep).ToList();
            array.Clear();
            array.AddRange(tempArray);
            return true;
        }
    }
}
