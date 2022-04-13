using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringMatchesRegexNode", "String Matches Regex", NodeTypeEnum.Condition, "String")]
    [NodeGraphDescription("Check if a string matches a regular expression")]
    [NodeIDEParameters(Hidden = true)]
    public class StringMatchesRegexNode : Node
    {
        public StringMatchesRegexNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringMatchesRegexNode).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));
            this.InParameters.Add("regex", new NodeParameter(this, "regex", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "true", new NodeParameter(this, "true", typeof(Node), false) },
                { "false", new NodeParameter(this, "false", typeof(Node), false) }
            };
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => false;

        public override bool OnExecution()
        {
            var original = this.InParameters["string"].GetValue().ToString();
            var regex = this.InParameters["regex"].GetValue().ToString();

                Regex r = new Regex(@regex);

                if (r.Match(original).Success)
                {
                    return (this.OutParameters["true"].Value as Node).Execute();
                }

            return (this.OutParameters["false"].Value as Node).Execute();
        }
    }
}
