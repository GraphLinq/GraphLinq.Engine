using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes
{
    [NodeDefinition("SecretStringNode", "Secret String", NodeTypeEnum.Variable, "Base Variable")]
    [NodeGraphDescription("A string that value are hidden in the IDE")]
    [NodeIDEParameters(IsSecretInput = true)]
    public class SecretStringNode : Node
    {
        public SecretStringNode(string id, BlockGraph graph)
          : base(id, graph, typeof(SecretStringNode).Name)
        {
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(string), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
