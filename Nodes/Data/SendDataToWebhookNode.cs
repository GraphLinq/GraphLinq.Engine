using Newtonsoft.Json;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Engine.Nodes.Data
{
    [NodeDefinition("SendDataToWebhookNode", "Send Data to Webhook", NodeTypeEnum.Function, "Data")]
    [NodeGraphDescription("Send data parameters as JSON to the url specified")]
    public class SendDataToWebhookNode : Node
    {
        private HttpClient client = new HttpClient();

        public SendDataToWebhookNode(string id, BlockGraph graph)
          : base(id, graph, typeof(SendDataToWebhookNode).Name)
        {
            this.InParameters.Add("url", new NodeParameter(this, "url", typeof(string), true));
            this.InParameters.Add("data", new NodeParameter(this, "data", typeof(object), true));

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var url = this.InParameters["url"].GetValue().ToString();
            var data = this.InParameters["data"].GetValue();
            var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            client.PostAsync(url, content).Wait();
            return true;
        }
    }
}
