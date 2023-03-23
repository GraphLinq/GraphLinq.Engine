using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Storage;
using NodeBlock.Engine.Storage.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Storage
{
    [NodeDefinition("KeyItemExistNode", "Is Key Item Exist", NodeTypeEnum.Function, "Storage")]
    [NodeGraphDescription("Check if a specific key from the Redis storage exist")]
    public class KeyItemExistNode : Node
    {
        public KeyItemExistNode(string id, BlockGraph graph)
           : base(id, graph, typeof(KeyItemExistNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "key", new NodeParameter(this, "key", typeof(string), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "true", new NodeParameter(this, "true", typeof(Node), false) },
                { "false", new NodeParameter(this, "false", typeof(Node), false) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            if (StorageManager.GetStorage().GraphKeyItemExist(this.Graph, this.InParameters["key"].GetValue().ToString()))
            {
                if (this.OutParameters["true"].Value == null) return true;
                return (this.OutParameters["true"].Value as Node).Execute();
            }
            else
            {
                if (this.OutParameters["false"].Value == null) return true;
                return (this.OutParameters["false"].Value as Node).Execute();
            }
        }
    }
}
