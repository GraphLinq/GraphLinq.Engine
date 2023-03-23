using System.Net.WebSockets;
using System.Threading.Tasks;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Engine.Nodes.WebSocket
{
    [NodeDefinition("WebSocketClientCloseNode", "WebSocket Client Close", NodeTypeEnum.Function, "WebSocket")]
    [NodeGraphDescription("Close a WebSocket client connection")]
    public class WebSocketClientCloseNode : Node
    {
        public WebSocketClientCloseNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WebSocketClientCloseNode).Name)
        {
            this.InParameters.Add("webSocketClient", new NodeParameter(this, "webSocketClient", typeof(WebSocketClientConnectorNode), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var webSocketClient = this.InParameters["webSocketClient"].GetValue() as WebSocketClientConnectorNode;

            if (webSocketClient.WebSocketClient.State == System.Net.WebSockets.WebSocketState.Open)
            {
                webSocketClient.WebSocketClient.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closing from node", System.Threading.CancellationToken.None).GetAwaiter().GetResult();
                return true;
            }

            return false;
        }
    }
}
