using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringGetAllMatchUsingRegexNode", "String Get All Match Using Regex", NodeTypeEnum.Condition, "String")]
    [NodeGraphDescription("Get All Matches in a string using regular expression")]
    [NodeIDEParameters(Hidden = false)]
    public class StringGetAllMatchUsingRegexNode : Node
    {
        public StringGetAllMatchUsingRegexNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringGetAllMatchUsingRegexNode).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));
            this.InParameters.Add("regex", new NodeParameter(this, "regex", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "each", new NodeParameter(this, "each", typeof(Node), false) },
                { "item", new NodeParameter(this, "item", typeof(object), false) }
            };
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var original = this.InParameters["string"].GetValue().ToString();
            var regex = this.InParameters["regex"].GetValue().ToString();

            var eachNode = this.OutParameters["each"].Value as Node;

            var array = Regex.Matches(original, regex);

            foreach (var obj in array)
            {
                this.OutParameters["item"].SetValue(obj);
                eachNode.Execute();
            }
            return true;
        }
    }
}
