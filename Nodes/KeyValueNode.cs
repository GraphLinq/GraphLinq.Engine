using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("KeyValueNode", "KeyValue", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("A keyValue is a data structure to associate key with a value.")]
    public class KeyValueNode : Node
    {
        public KeyValueNode(string id, BlockGraph graph)
          : base(id, graph, typeof(StringNode).Name)
        {

            this.InParameters.Add("key", new NodeParameter(this, "key", typeof(object), true));
            this.InParameters.Add("value", new NodeParameter(this, "value", typeof(object), true));

            this.OutParameters.Add("output", new NodeParameter(this, "output", typeof(KeyValuePair<object, object>), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override bool OnExecution()
        {
            this.OutParameters["output"].SetValue(new KeyValuePair<object,object>(this.InParameters["key"],this.InParameters["value"]));
            return true;
        }
    }
}
