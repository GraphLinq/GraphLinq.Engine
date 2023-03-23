using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.WebSocket
{
    [NodeDefinition("WebSocketClientOnDisconnectNode", "On WebSocket Client Disconnect", NodeTypeEnum.Event, "WebSocket")]
    [NodeGraphDescription("Triggered when the WebSocket client is disconnected from the server")]
    public class WebSocketClientOnDisconnectNode : Node
    {
        public WebSocketClientOnDisconnectNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WebSocketClientOnDisconnectNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("webSocketClient", new NodeParameter(this, "webSocketClient", typeof(WebSocketClientConnectorNode), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            WebSocketClientConnectorNode clientNode = this.InParameters["webSocketClient"].GetValue() as WebSocketClientConnectorNode;

            clientNode.OnClose += ClientNode_OnClose;
        }

        private void ClientNode_OnClose(object sender, EventArgs e)
        {
            WebSocketClientConnectorNode clientNode = this.InParameters["webSocketClient"].GetValue() as WebSocketClientConnectorNode;
            clientNode.OnClose -= ClientNode_OnClose;
            var instanciatedParameters = this.InstanciateParametersForCycle();
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
