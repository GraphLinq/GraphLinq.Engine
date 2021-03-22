using Newtonsoft.Json.Linq;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("AddJsonValueNode", "Add JSON Property", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Add a property into a JSON Object")]
    public class AddJsonValueNode : Node
    {
        public AddJsonValueNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AddJsonValueNode).Name)
        {
            this.InParameters.Add("jsonObject", new NodeParameter(this, "jsonObject", typeof(object), true));
            this.InParameters.Add("key", new NodeParameter(this, "key", typeof(string), true));
            this.InParameters.Add("value", new NodeParameter(this, "value", typeof(string), true));
        }

        public override bool CanExecute => true;

        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var jsonObject = this.InParameters["jsonObject"].GetValue() as JObject;
            var key = this.InParameters["key"].GetValue().ToString();
            var value = this.InParameters["value"].GetValue();
            jsonObject.Add(new JProperty(key, value));
            return true;
        }
    }
}
