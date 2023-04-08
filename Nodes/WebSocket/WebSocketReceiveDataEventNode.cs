using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Engine.Nodes.WebSocket
{
    [NodeDefinition("WebSocketReceiveDataEventNode", "WebSocket Receive Data Event", NodeTypeEnum.Event, "WebSocket")]
    [NodeGraphDescription("Trigger events when data is received from the WebSocket server")]
    public class WebSocketReceiveDataEventNode : Node
    {
        public WebSocketReceiveDataEventNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WebSocketReceiveDataEventNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("webSocketClient", new NodeParameter(this, "webSocketClient", typeof(WebSocketClientConnectorNode), true));

            this.OutParameters.Add("data", new NodeParameter(this, "data", typeof(string), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => false;

        public override void SetupEvent()
        {
            WebSocketClientConnectorNode webSocketClientNode = this.InParameters["webSocketClient"].GetValue() as WebSocketClientConnectorNode;
            webSocketClientNode.OnDataReceived += WebSocketClientNode_OnDataReceived;
        }

        public override void OnStop()
        {
            WebSocketClientConnectorNode webSocketClientNode = this.InParameters["webSocketClient"].GetValue() as WebSocketClientConnectorNode;
            webSocketClientNode.OnDataReceived -= WebSocketClientNode_OnDataReceived;
        }

        private void WebSocketClientNode_OnDataReceived(object sender, string data)
        {
            var instanciatedParameters = this.InstanciatedParametersForCycle();
            instanciatedParameters["data"].SetValue(data);
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
