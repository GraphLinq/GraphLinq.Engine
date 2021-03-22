using Newtonsoft.Json;
using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Nodes.Encoding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeBlock.Engine.Nodes.Data
{
    [NodeDefinition("ConvertNodeToDataNode", "Convert To Data", NodeTypeEnum.Function, "Data")]
    [NodeGraphDescription("Convert output parameters to data parameters")]
    public class ConvertNodeToDataNode : Node
    {
        public ConvertNodeToDataNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ConvertNodeToDataNode).Name)
        {
            this.OutParameters.Add("data", new NodeParameter(this, "data", typeof(object), true));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            if (this.LastExecutionFrom.CanBeSerialized)
            {
                this.OutParameters["data"].Value = this.LastExecutionFrom.OutParameters.Values.ToList().Select(x => new KeyValueData
                {
                    Key = x.Name,
                    Value = x.GetValue()
                });
            }
            else
            {
                this.OutParameters["data"].Value = new { error = "This node can't be serialized" };
            }
            return true;
        }
    }
}
