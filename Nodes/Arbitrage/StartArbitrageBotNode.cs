using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Nodes.Bot;
using System.Globalization;

namespace NodeBlock.Engine.Nodes.Arbitrage
{
    [NodeDefinition("StartArbitrageBotNode", "Start Arbitrage Bot", NodeTypeEnum.Function, "Dextools")]
    [NodeGraphDescription("Start the Arbitrage bot")]
    public class StartArbitrageBotNode : Node
    {
        public StartArbitrageBotNode(string id, BlockGraph graph)
             : base(id, graph, typeof(StartArbitrageBotNode).Name)
        {
            CanBeSerialized = false;
            InParameters.Add("privateKey", new NodeParameter(this, "privateKey", typeof(string), true));
            InParameters.Add("amountEthToSwap", new NodeParameter(this, "amountEthToSwap", typeof(double), true));
            InParameters.Add("amountGlqToSwap", new NodeParameter(this, "amountGlqToSwap", typeof(double), true));
            InParameters.Add("botDeadlineInSeconds", new NodeParameter(this, "botDeadlineInSeconds", typeof(int), true));
            OutParameters.Add("arbitrageBotId", new NodeParameter(this, "arbitrageBotId", typeof(string), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var privateKey = InParameters["privateKey"].GetValue().ToString();
            var amountEthToSwap = double.Parse(InParameters["amountEthToSwap"].GetValue().ToString(), CultureInfo.InvariantCulture);
            var amountGlqToSwap = double.Parse(InParameters["amountGlqToSwap"].GetValue().ToString(), CultureInfo.InvariantCulture);
            var secondsToRetryAttempt = int.Parse(InParameters["botDeadlineInSeconds"].GetValue().ToString());
            var botId = ArbitrageBotManager.Instance.StartBot(privateKey, amountEthToSwap, amountGlqToSwap, secondsToRetryAttempt, Graph);
            OutParameters["arbitrageBotId"].SetValue(botId);
            if (botId == null) return false;
            ArbitrageBotManager.Instance.StartKeepAlive(botId, Graph);
            return true;
        }
    }
}
