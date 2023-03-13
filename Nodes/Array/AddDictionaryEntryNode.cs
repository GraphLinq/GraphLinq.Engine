using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("AddDictionaryEntry", "Add Dictionary Entry", NodeTypeEnum.Function, "Dictionary")]
    [NodeGraphDescription("Add a entry with a key")]
    public class AddDictionaryEntry : Node
    { 
        public AddDictionaryEntry(string id, BlockGraph graph)
           : base(id, graph, typeof(AddDictionaryEntry).Name)
        {
            this.InParameters.Add("dictionary", new NodeParameter(this, "dictionary", typeof(object), true));
            this.InParameters.Add("key", new NodeParameter(this, "key", typeof(string), true));
            this.InParameters.Add("element", new NodeParameter(this, "element", typeof(object), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var array = this.InParameters["dictionary"].GetValue() as Dictionary<string, object>;
            array[this.InParameters["key"].GetValue().ToString()] = this.InParameters["element"].GetValue();
            return true;
        }
    }
}
