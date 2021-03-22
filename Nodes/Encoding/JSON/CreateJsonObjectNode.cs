using Newtonsoft.Json.Linq;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("CreateJsonObjectNode", "Create JSON Object", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Create a new empty JSON object")]
    public class CreateJsonObjectNode : Node
    {
        public CreateJsonObjectNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateJsonObjectNode).Name)
        {
            this.OutParameters.Add("jsonObject", new NodeParameter(this, "jsonObject", typeof(object), false));
        }

        public JObject JsonObject;

        public override bool CanExecute => true;

        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            this.JsonObject = new JObject();
            this.OutParameters["jsonObject"].SetValue(this.JsonObject);
            return true;
        }
    }
}
