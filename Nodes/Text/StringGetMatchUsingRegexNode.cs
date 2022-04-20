using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringGetMatchUsingRegexNode", "String Get Match Using Regex", NodeTypeEnum.Condition, "String")]
    [NodeGraphDescription("Extract match in string using regular expression")]
    [NodeIDEParameters(Hidden = false)]
    public class StringGetMatchUsingRegexNode : Node
    {
        public StringGetMatchUsingRegexNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringGetMatchUsingRegexNode).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));
            this.InParameters.Add("regex", new NodeParameter(this, "regex", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "returnText", new NodeParameter(this, "returnText", typeof(string), false) }
            };
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var original = this.InParameters["string"].GetValue().ToString();
            var regex = this.InParameters["regex"].GetValue().ToString();

                Regex r = new Regex(@regex);

                if (r.Match(original).Success)
                {
                    return (this.OutParameters["returnText"].SetValue(r.Match(original).Value));
                }

            return (this.OutParameters["returnText"].SetValue(""));
        }
    }
}
