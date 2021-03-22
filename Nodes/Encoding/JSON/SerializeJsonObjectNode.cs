using Newtonsoft.Json.Linq;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("SerializeJsonObjectNode", "Serialize JSON Object", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Serialize a JSON object to a string")]
    public class SerializeJsonObjectNode : Node
    {
        public SerializeJsonObjectNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SerializeJsonObjectNode).Name)
        {
            this.InParameters.Add("jsonObject", new NodeParameter(this, "jsonObject", typeof(object), true));
            this.OutParameters.Add("json", new NodeParameter(this, "json", typeof(string), false));
        }

        public override bool CanExecute => true;

        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var jsonObject = this.InParameters["jsonObject"].GetValue() as JObject;
            this.OutParameters["json"].SetValue(jsonObject.ToString());
            return true;
        }
    }
}
