using Newtonsoft.Json;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding
{
    [NodeDefinition("ConvertToJsonNode", "Convert To JSON", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Convert the received any type parameter into a JSON object readable")]
    public class ConvertToJsonNode : Node
    {
        public ConvertToJsonNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ConvertToJsonNode).Name)
        {
            this.InParameters.Add("object", new NodeParameter(this, "object", typeof(object), true));

            this.OutParameters.Add("json", new NodeParameter(this, "json", typeof(string), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "json")
            {
                return JsonConvert.SerializeObject(this.InParameters["object"].GetValue());
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
