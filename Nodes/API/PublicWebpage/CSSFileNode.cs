using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API.PublicWebpage
{
    [NodeDefinition("CSSFileNode", "CSS File", NodeTypeEnum.Variable, "Public Web Page")]
    [NodeGraphDescription("A string that contain a CSS file")]
    [NodeIDEParameters(IsScriptInput = true, ScriptType = "css")]
    public class CSSFileNode : Node
    {
        public CSSFileNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CSSFileNode).Name)
        {
            this.OutParameters.Add("css", new NodeParameter(this, "css", typeof(string), true));
        }

        public override bool CanExecute => false;
        public override bool CanBeExecuted => false;
    }
}