using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Storage.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Storage
{
    [NodeDefinition("GetWalletKeyItemNode", "Get Wallet Key Item", NodeTypeEnum.Function, "Storage")]
    [NodeGraphDescription("Return a specific key from the Redis storage allocated for the wallet context")]
    [NodeGasConfiguration("100000000000000")]
    public class GetWalletKeyItemNode : Node
    {
        public GetWalletKeyItemNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetWalletKeyItemNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "key", new NodeParameter(this, "key", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "value", new NodeParameter(this, "value", typeof(string), true) }
            };
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "value")
            {
                var v = RedisStorage.GetWalletGraphKeyItem(this.Graph, this.InParameters["key"].GetValue().ToString());
                return v;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
