using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Engine.Nodes.WebSocket
{
    [NodeDefinition("WebSocketSendDataNode", "WebSocket Send Data", NodeTypeEnum.Function, "WebSocket")]
    [NodeGraphDescription("Send data to a connected WebSocket server")]
    public class WebSocketSendDataNode : Node
    {
        public WebSocketSendDataNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WebSocketSendDataNode).Name)
        {
            this.InParameters.Add("webSocketClient", new NodeParameter(this, "webSocketClient", typeof(WebSocketClientConnectorNode), true));
            this.InParameters.Add("data", new NodeParameter(this, "data", typeof(string), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var webSocketClientNode = this.InParameters["webSocketClient"].GetValue() as WebSocketClientConnectorNode;
            var data = this.InParameters["data"].GetValue().ToString();

            return SendData(webSocketClientNode.WebSocketClient, data);
        }

        private bool SendData(ClientWebSocket webSocketClient, string data)
        {
            try
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(data);
                var sendBuffer = new ArraySegment<byte>(buffer);
                webSocketClient.SendAsync(sendBuffer, WebSocketMessageType.Text, true, CancellationToken.None).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                // Handle sending errors here
                return false;
            }
        }
    }
}
