using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Text
{
    [NodeDefinition("StringSplitNode", "String Split", NodeTypeEnum.Function, "String")]
    [NodeGraphDescription("Split String By Character")]
    [NodeIDEParameters(Hidden = false)]
    public class StringSplitNode : Node
    {
        public StringSplitNode(string id, BlockGraph graph)
            : base(id, graph, typeof(StringSplitNode).Name)
        {
            this.InParameters.Add("original", new NodeParameter(this, "original", typeof(string), true));
            this.InParameters.Add("splitUsing", new NodeParameter(this, "splitUsing", typeof(string), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "each", new NodeParameter(this, "each", typeof(Node), false) },
                { "item", new NodeParameter(this, "item", typeof(object), false) },
            };
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {


            if (this.OutParameters["each"].Value == null) return true;
            var array = this.InParameters["original"].GetValue().ToString().Split(this.InParameters["splitUsing"].GetValue().ToString());

            var eachNode = this.OutParameters["each"].Value as Node;
            foreach (var obj in array)
            {
                this.OutParameters["item"].SetValue(obj);
                eachNode.Execute();
            }
            return true;
        }
    }
}
