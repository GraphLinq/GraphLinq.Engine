using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API.PublicWebpage
{
    [NodeDefinition("JavascriptFileNode", "Javascript File", NodeTypeEnum.Variable, "Public Web Page")]
    [NodeGraphDescription("A string that contain a Javascript file")]
    [NodeIDEParameters(IsScriptInput = true, ScriptType = "javascript")]
    public class JavascriptFileNode : Node
    {
        public JavascriptFileNode(string id, BlockGraph graph)
            : base(id, graph, typeof(JavascriptFileNode).Name)
        {
            this.OutParameters.Add("javascript", new NodeParameter(this, "javascript", typeof(string), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}
