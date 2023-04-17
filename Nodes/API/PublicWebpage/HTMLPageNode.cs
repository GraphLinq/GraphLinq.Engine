using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API.PublicWebpage
{
    [NodeDefinition("HTMLPageNode", "HTML Page", NodeTypeEnum.Variable, "Public Web Page")]
    [NodeGraphDescription("A string that contain a HTML Page")]
    [NodeIDEParameters(IsScriptInput = true, ScriptType = "html")]
    public class HTMLPageNode : Node
    {
        public HTMLPageNode(string id, BlockGraph graph)
            : base(id, graph, typeof(HTMLPageNode).Name)
        {
            this.OutParameters.Add("html", new NodeParameter(this, "html", typeof(string), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
