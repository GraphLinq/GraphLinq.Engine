using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("SubstringNode", "Substring", NodeTypeEnum.Function, "String")]
    [NodeGraphDescription("Substring a text")]
    public class SubstringNode : Node
    {
        public SubstringNode(string id, BlockGraph graph)
                  : base(id, graph, typeof(SubstringNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "input", new NodeParameter(this, "input", typeof(string), true) },
                { "startIndex", new NodeParameter(this, "startIndex", typeof(int), true) }
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
                var input = this.InParameters["input"].GetValue().ToString();
                var startIndex = int.Parse(this.InParameters["startIndex"].GetValue().ToString());

                return input.Substring(startIndex);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
