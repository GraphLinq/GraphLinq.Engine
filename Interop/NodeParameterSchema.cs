using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Interop
{
    public class NodeParameterSchema
    {
        [JsonIgnore]
        private readonly Node node;
        [JsonIgnore]
        private NodeParameter nodeParameter;

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }

        [JsonProperty(PropertyName = "assignment")]
        public string Assignment { get; set; }

        [JsonProperty(PropertyName = "assignment_node")]
        public string AssignmentNode { get; set; }

        [JsonProperty(PropertyName = "value_is_reference")]
        public bool ValueIsReference;

        public NodeParameterSchema() { }
        public NodeParameterSchema(Node node, NodeParameter nodeParameter)
        {
            this.node = node;
            this.nodeParameter = nodeParameter;
            this.Id = this.nodeParameter.Id;
            this.Name = this.nodeParameter.Name;
            this.Type = this.nodeParameter.ValueType.FullName;
            if(!nodeParameter.IsDynamic)
            {
                if (nodeParameter.ValueType == typeof(Node))
                {
                    if (this.nodeParameter.Value != null)
                    {
                        this.Value = (this.nodeParameter.Value as Node).Id;
                    }
                    this.ValueIsReference = true;
                }
                else
                {
                    this.ValueIsReference = false;
                    this.Value = this.nodeParameter.Value;
                }
            }
            this.Assignment = this.nodeParameter.Assignments != null ? this.nodeParameter.Assignments.Id : string.Empty;
            this.AssignmentNode = this.nodeParameter.Assignments != null ? node.Graph.FindParameterNodeById(this.nodeParameter.Assignments.Id).Id : string.Empty;
        }
    }
}
