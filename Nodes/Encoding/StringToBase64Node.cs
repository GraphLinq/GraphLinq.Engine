using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding
{
    [NodeDefinition("StringToBase64Node", "String to Base64", NodeTypeEnum.Function, "Transformers")]
    [NodeGraphDescription("Transform a clear string into a base64 string as output")]
    public class StringToBase64Node : Node
    {
        public StringToBase64Node(string id, BlockGraph graph)
            : base(id, graph, typeof(StringToBase64Node).Name)
        {
            this.InParameters.Add("string", new NodeParameter(this, "string", typeof(string), true));

            this.OutParameters.Add("base64", new NodeParameter(this, "base64", typeof(string), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "base64")
            {
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.InParameters["string"].GetValue().ToString()));
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
