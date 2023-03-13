using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Array
{
    [NodeDefinition("GetDictionaryEntryNode", "Get Dictionary Entry", NodeTypeEnum.Function, "Dictionary")]
    [NodeGraphDescription("Get a entry with a key")]
    public class GetDictionaryEntryNode : Node
    {
        public GetDictionaryEntryNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetDictionaryEntryNode).Name)
        {
            this.InParameters.Add("dictionary", new NodeParameter(this, "dictionary", typeof(object), true));
            this.InParameters.Add("key", new NodeParameter(this, "key", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "entry", new NodeParameter(this, "entry", typeof(object), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "entry")
            {
                var array = this.InParameters["dictionary"].GetValue() as Dictionary<string, object>;
                return array[this.InParameters["key"].GetValue().ToString()];
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
