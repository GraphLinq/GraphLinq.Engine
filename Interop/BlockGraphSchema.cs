using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Interop
{
    public class BlockGraphSchema
    {
        [JsonIgnore]
        private BlockGraph graph { get; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "nodes")]
        public List<NodeSchema> Nodes { get; set; }

        [JsonProperty(PropertyName = "rawDeps")]
        public List<string>RawDeps { get; set; }

        public BlockGraphSchema() { }
        public BlockGraphSchema(BlockGraph graph)
        {
            this.graph = graph;
            this.Nodes = new List<NodeSchema>();
        }

        public string Export()
        {
            this.Name = graph.Name;
            foreach(var node in this.graph.Nodes)
            {
                var nodeSchema = new NodeSchema(node.Value);
                this.Nodes.Add(nodeSchema);
            }
            return JsonConvert.SerializeObject(this);
        }
    }
}
