using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringReplaceUsingRegexNode", "String Replace Using Regex", NodeTypeEnum.Function, "String")]
    [NodeGraphDescription("Replace match in string using regular expression")]
    [NodeIDEParameters(Hidden = false)]
    public class StringReplaceUsingRegexNode : Node
    {
        public StringReplaceUsingRegexNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringReplaceUsingRegexNode).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));
            this.InParameters.Add("regex", new NodeParameter(this, "regex", typeof(string), true));
            this.InParameters.Add("replace", new NodeParameter(this, "replace", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "returnText", new NodeParameter(this, "returnText", typeof(string), false) }
            };
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "returnText")
            {
                var original = this.InParameters["string"].GetValue().ToString();
                var regex = this.InParameters["regex"].GetValue().ToString();
                var replace = this.InParameters["replace"].GetValue().ToString();
                return Regex.Replace(original, regex, replace);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
