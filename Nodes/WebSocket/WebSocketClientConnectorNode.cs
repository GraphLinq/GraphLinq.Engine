using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;


namespace NodeBlock.Engine.Nodes.WebSocket
{
    [NodeDefinition("WebSocketClientConnectorNode", "WebSocket Client Connector", NodeTypeEnum.Connector, "WebSocket")]
    [NodeGraphDescription("Create a WebSocket client that connects to a WebSocket server")]
    public class WebSocketClientConnectorNode : Node
    {
        public WebSocketClientConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WebSocketClientConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("uri", new NodeParameter(this, "uri", typeof(string), true));
            this.OutParameters.Add("webSocketClient", new NodeParameter(this, "webSocketClient", typeof(WebSocketClientConnectorNode), true));
            this.OutParameters.Add("onError", new NodeParameter(this, "onError", typeof(Node), false));
        }

        public event EventHandler<string> OnDataReceived;
        public event EventHandler OnClose;

        public string Uri { get; set; }
        public ClientWebSocket WebSocketClient { get; set; }

        public override bool CanBeExecuted => false;
        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            Uri = this.InParameters["uri"].GetValue().ToString();
            WebSocketClient = new ClientWebSocket();

            bool connected = ConnectToWebSocketServer();

            if (connected)
            {
                Task.Run(() => ReceiveDataAsync(WebSocketClient));
                this.Next();
            }
            else
            {
                TriggerOnErrorNode();
            }
        }

        private bool ConnectToWebSocketServer()
        {
            try
            {
                WebSocketClient.ConnectAsync(new Uri(Uri), CancellationToken.None).GetAwaiter().GetResult();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        private void TriggerOnErrorNode()
        {
            if (this.OutParameters["onError"].Value == null) return;
            (this.OutParameters["onError"].Value as Node).Execute();
        }

        private async Task ReceiveDataAsync(ClientWebSocket clientWebSocket)
        {
            var buffer = new byte[1024];

            while (clientWebSocket.State == WebSocketState.Open)
            {
                var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var data = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    OnDataReceived?.Invoke(this, data);
                }
            }
            if (clientWebSocket.State == WebSocketState.Closed)
            {
                OnClose?.Invoke(this, EventArgs.Empty);
            }
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "webSocketClient")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
