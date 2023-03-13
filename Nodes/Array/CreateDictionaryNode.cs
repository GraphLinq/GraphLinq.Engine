using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("CreateDictionaryNode", "Create Dictionary", NodeTypeEnum.Function, "Dictionary")]
    [NodeGraphDescription("An Dictionary can store multiple variable in it with a key")]

    public class CreateDictionaryNode : Node
    {
        public CreateDictionaryNode(string id, BlockGraph graph)
           : base(id, graph, typeof(CreateDictionaryNode).Name)
        {
            this.OutParameters.Add("dictionary", new NodeParameter(this, "dictionary", typeof(object), true));
        }

        public Dictionary<string, object> Array { get; set; }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            this.Array = new Dictionary<string, object>();
            this.OutParameters["dictionary"].SetValue(this.Array);
            return true;
        }
    }
}
