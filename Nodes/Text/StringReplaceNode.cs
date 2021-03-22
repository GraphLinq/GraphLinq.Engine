using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringReplaceNode", "Replace String in String", NodeTypeEnum.Function, "String")]
    [NodeGraphDescription("Replace some part of the original string by a new strings")]
    public class StringReplaceNode : Node
    {
        public StringReplaceNode(string id, BlockGraph graph)
                : base(id, graph, typeof(StringReplaceNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "original", new NodeParameter(this, "original", typeof(string), true) },
                { "toReplace", new NodeParameter(this, "toReplace", typeof(string), true) },
                { "replaceText", new NodeParameter(this, "replaceText", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "string", new NodeParameter(this, "string", typeof(string), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "string")
            {
                var original = this.InParameters["original"].GetValue().ToString();
                var toReplace = this.InParameters["toReplace"].GetValue().ToString();
                var replaceText = this.InParameters["replaceText"].GetValue().ToString();
                return original.Replace(toReplace, replaceText);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
