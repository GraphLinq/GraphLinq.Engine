using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("EachElementArrayNode", "Each Element In Array", NodeTypeEnum.Function, "Array")]
    [NodeGraphDescription("Loop on all element in a array")]
    public class EachElementArrayNode : Node
    {
        public EachElementArrayNode(string id, BlockGraph graph)
            : base(id, graph, typeof(EachElementArrayNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "array", new NodeParameter(this, "array", typeof(List<object>), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "each", new NodeParameter(this, "each", typeof(Node), false) },
                { "item", new NodeParameter(this, "item", typeof(object), false) },
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (this.OutParameters["each"].Value == null) return true;
            var array = this.InParameters["array"].GetValue() as List<object>;
            var eachNode = this.OutParameters["each"].Value as Node;
            foreach (var obj in array)
            {
                this.OutParameters["item"].SetValue(obj);
                eachNode.Execute();
            }
            return true;
        }
    }
}
