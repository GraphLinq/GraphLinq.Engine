using Newtonsoft.Json;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("SerializeToJsonNode", "Serialize To JSON", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Serialize a value to JSON format")]
    public class SerializeToJsonNode : Node
    {
        public SerializeToJsonNode(string id, BlockGraph graph)
         : base(id, graph, typeof(SerializeToJsonNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(object), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "json", new NodeParameter(this, "json", typeof(string), false, null, "", true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "json")
            {
                var v = this.InParameters["value"].GetValue();
                return JsonConvert.SerializeObject(v);
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
