using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.HostedAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.API.PublicWebpage
{
    [NodeDefinition("ProcessTemplateNode", "Process Template", NodeTypeEnum.Function, "Hosted API")]
    [NodeGraphDescription("Retrive all the vars from the running graph and replace by them in the web page template")]
    public class ProcessTemplateNode : Node
    {
        public ProcessTemplateNode(string id, BlockGraph graph)
           : base(id, graph, typeof(ProcessTemplateNode).Name)
        {
            this.InParameters.Add("html", new NodeParameter(this, "html", typeof(string), true));

            this.OutParameters.Add("htmlProcessed", new NodeParameter(this, "htmlProcessed", typeof(string), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var html = this.InParameters["html"].GetValue().ToString();

            var varsToReplace = Utils.StringUtils.ExtractTextWithinDoubleCurlyBraces(html);
            foreach(var v in varsToReplace)
            {
                if (!this.Graph.MemoryVariables.ContainsKey(v)) continue;
                var memoryVariable = this.Graph.MemoryVariables[v];
                html = html.Replace("{{" + v + "}}", memoryVariable.ToString());
            }
            this.OutParameters["htmlProcessed"].SetValue(html);

            return true;
        }
    }
}
