using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("HasKeyInDictionaryNode", "Has Key In Dictionary", NodeTypeEnum.Condition, "Dictionary")]
    [NodeGraphDescription("Check if a key exist in the dictionary")]
    public class HasKeyInDictionaryNode : Node
    {
        public HasKeyInDictionaryNode(string id, BlockGraph graph)
            : base(id, graph, typeof(HasKeyInDictionaryNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "dictionary", new NodeParameter(this, "dictionary", typeof(object), true) },
                { "key", new NodeParameter(this, "key", typeof(string), true) }
            };
            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "true", new NodeParameter(this, "true", typeof(Node), false) },
                { "false", new NodeParameter(this, "false", typeof(Node), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var array = this.InParameters["dictionary"].GetValue() as Dictionary<string, object>;
            var key = this.InParameters["key"].GetValue().ToString();


            if (array.ContainsKey(key))
            {
                if (this.OutParameters["true"].Value == null) return true;
                return (this.OutParameters["true"].Value as Node).Execute();
            }
            else
            {
                if (this.OutParameters["false"].Value == null) return true;
                return (this.OutParameters["false"].Value as Node).Execute();
            }
        }
    }
}
