using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Storage.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Storage
{
    [NodeDefinition("SaveKeyItemNode", "Save Key Item", NodeTypeEnum.Function, "Storage")]
    [NodeGraphDescription("Save a specific key in the Redis storage allocated for the graph execution")]
    [NodeGasConfiguration("1000000000000000")]
    public class SaveKeyItemNode : Node
    {
        public SaveKeyItemNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SaveKeyItemNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "key", new NodeParameter(this, "key", typeof(string), true) },
                { "value", new NodeParameter(this, "value", typeof(object), true) },
                { "toJson", new NodeParameter(this, "toJson", typeof(bool), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {

            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var value = this.InParameters["value"].GetValue().ToString();
            RedisStorage.SetGraphKeyItem(this.Graph, this.InParameters["key"].GetValue().ToString(), value);
            return true;
        }
    }
}
