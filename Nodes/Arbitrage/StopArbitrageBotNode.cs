using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Nodes.Bot;

namespace NodeBlock.Engine.Nodes.Arbitrage
{
    [NodeDefinition("StopArbitrageBotNode", "Stop Arbitrage Bot", NodeTypeEnum.Function, "Dextools")]
    [NodeGraphDescription("Stop the Arbitrage bot")]
    public class StopArbitrageBotNode : Node
    {

        public StopArbitrageBotNode(string id, BlockGraph graph)
             : base(id, graph, typeof(StopArbitrageBotNode).Name)
        {
            CanBeSerialized = false;

            InParameters.Add("arbitrageBotId", new NodeParameter(this, "arbitrageBotId", typeof(string), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var arbitrageBotId = InParameters["arbitrageBotId"].GetValue().ToString();
            ArbitrageBotManager.Instance.StopBot(arbitrageBotId, Graph);

            return true;
        }

    }
}
