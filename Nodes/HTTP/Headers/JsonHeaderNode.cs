﻿using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Engine.Nodes.HTTP.Headers
{
    [NodeDefinition("JsonHeaderNode", "Json Header Node", NodeTypeEnum.Function, "HTTP")]
    [NodeGraphDescription("Convert the json data into a header for a http request.")]
    public class JsonHeaderNode : Node
    {

        public JsonHeaderNode(string id, BlockGraph graph)
            : base(id, graph, typeof(JsonHeaderNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "json", new NodeParameter(this, "json", typeof(string), true) },
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
                { "header", new NodeParameter(this, "header", typeof(HttpContent), false, null, "", true) },
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            this.OutParameters["header"].SetValue(new System.Net.Http.StringContent(this.InParameters["json"].GetValue().ToString(), System.Text.Encoding.UTF8, "application/json"));       
            return true;
        }
    }
}