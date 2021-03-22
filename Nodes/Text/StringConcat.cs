using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringConcat", "Concat String", NodeTypeEnum.Function, "String")]
    [NodeGraphDescription("Split two string by a specific delimiter and return it as out parameter.")]
    public class StringConcat : Node
    {
        public StringConcat(string id, BlockGraph graph)
                  : base(id, graph, typeof(StringConcat).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "stringA", new NodeParameter(this, "stringA", typeof(string), true) },
                { "stringB", new NodeParameter(this, "stringB", typeof(string), true) },
                { "delimiter", new NodeParameter(this, "delimiter", typeof(string), true) }
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
                var varA = this.InParameters["stringA"].GetValue().ToString();
                var varB = this.InParameters["stringB"].GetValue().ToString();
                var delimiter = this.InParameters["delimiter"].GetValue();
                if(delimiter == null)
                {
                    delimiter = " ";
                }

                return varA + delimiter.ToString() + varB;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
