using Newtonsoft.Json;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("JsonDeserializeToArrayNode", "JSON Deserialize To Array", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Convert a plain json string to a array")]
    public class JsonDeserializeToArrayNode : Node
    {
        public JsonDeserializeToArrayNode(string id, BlockGraph graph)
            : base(id, graph, typeof(JsonDeserializeToArrayNode).Name)
        {
            this.InParameters.Add("json", new NodeParameter(this, "json", typeof(string), true));
            this.OutParameters.Add("array", new NodeParameter(this, "array", typeof(List<object>), false));
        }

        public override bool CanExecute => false;

        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "array")
            {
                var json = this.InParameters["json"].GetValue().ToString();
                var jsonObject = JsonConvert.DeserializeObject<List<object>>(json);
                return jsonObject;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
