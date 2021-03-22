using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace NodeBlock.Engine.Nodes.Encoding
{
    [NodeDefinition("ConvertLastBlockOutputToJsonNode", "Last Node To JSON", NodeTypeEnum.Function, "JSON")]
    [NodeGraphDescription("Serialize the last node executed in the graph to JSON data")]
    public class ConvertLastBlockOutputToJsonNode : Node
    {
        public ConvertLastBlockOutputToJsonNode(string id, BlockGraph graph)
           : base(id, graph, typeof(ConvertLastBlockOutputToJsonNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "json", new NodeParameter(this, "json", typeof(string), false, null, "", true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if(this.LastExecutionFrom.CanBeSerialized)
            {
                this.OutParameters["json"].Value = JsonConvert.SerializeObject(this.LastExecutionFrom.OutParameters.Values.ToList().Select(x => new
                {
                    key = x.Name,
                    value = x.GetValue()
                }));
            }
            else
            {
                this.OutParameters["json"].Value = new { error = "This node can't be serialized" };
            }
            return true;
        }
    }
}
