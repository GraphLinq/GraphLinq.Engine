using Newtonsoft.Json.Linq;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("JsonToJsonObjectNode", "JSON to JSON Object", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Convert a plain json string to a json object")]
    public class JsonToJsonObjectNode : Node
    {
        public JsonToJsonObjectNode(string id, BlockGraph graph)
            : base(id, graph, typeof(JsonToJsonObjectNode).Name)
        {
            this.InParameters.Add("json", new NodeParameter(this, "json", typeof(string), true));
            this.OutParameters.Add("jsonObject", new NodeParameter(this, "jsonObject", typeof(object), false));
        }

        public override bool CanExecute => false;

        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "jsonObject")
            {
                var jsonObject = JObject.Parse(this.InParameters["json"].GetValue().ToString());
                return jsonObject;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
