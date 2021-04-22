using Newtonsoft.Json.Linq;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Encoding.JSON
{
    [NodeDefinition("MergeJSONNode", "Merge JSON", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Merge two JSON into one")]
    public class MergeJSONNode : Node
    {
        public MergeJSONNode(string id, BlockGraph graph)
            : base(id, graph, typeof(MergeJSONNode).Name)
        {
            this.InParameters.Add("json1", new NodeParameter(this, "json1", typeof(string), true));
            this.InParameters.Add("json2", new NodeParameter(this, "json2", typeof(string), true));


            this.OutParameters.Add("mergedJson", new NodeParameter(this, "mergedJson", typeof(string), true));
        }

        public override bool CanExecute => true;

        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var json1 = this.InParameters["json1"].GetValue().ToString();
            var json2 = this.InParameters["json2"].GetValue().ToString();
            var jo1 = JObject.Parse(json1);
            var jo2 = JObject.Parse(json2);
            jo1.Merge(jo2, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
            this.OutParameters["mergedJson"].SetValue(jo1.ToString());
            return true;
        }
    }
}
