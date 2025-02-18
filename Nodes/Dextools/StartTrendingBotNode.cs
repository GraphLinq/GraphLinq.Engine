using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Nodes.Bot;
using System.Globalization;

namespace NodeBlock.Engine.Nodes.Dextools
{
    [NodeDefinition("StartTrendingBotNode", "Start Trending Bot", NodeTypeEnum.Function, "Dextools")]
    [NodeGraphDescription("Start the trending bot")]
    public class StartTrendingBotNode : Node
    {
        public StartTrendingBotNode(string id, BlockGraph graph)
             : base(id, graph, typeof(StartTrendingBotNode).Name)
        {
            this.CanBeSerialized = false;
            this.InParameters.Add("privateKey", new NodeParameter(this, "privateKey", typeof(string), true));
            this.InParameters.Add("amountEthToSwapForOneToken", new NodeParameter(this, "amountEthToSwapForOneToken", typeof(double), true));
            this.InParameters.Add("maxTokensAmount", new NodeParameter(this, "maxTokensAmount", typeof(double), true));
            this.InParameters.Add("dextoolsApiKey", new NodeParameter(this, "dextoolsApiKey", typeof(string), true));
            this.OutParameters.Add("trendingBotId", new NodeParameter(this, "trendingBotId", typeof(string), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var privateKey = this.InParameters["privateKey"].GetValue().ToString();
            var dextoolsApiKey = this.InParameters["dextoolsApiKey"].GetValue().ToString();
            var amountEthToSwapForOneToken = double.Parse(this.InParameters["amountEthToSwapForOneToken"].GetValue().ToString(), CultureInfo.InvariantCulture);
            var maxTokensAmount = double.Parse(this.InParameters["maxTokensAmount"].GetValue().ToString(), CultureInfo.InvariantCulture);
            var botId = TrendingBotManager.Instance.StartBot(privateKey, amountEthToSwapForOneToken, maxTokensAmount, dextoolsApiKey, Graph);
            this.OutParameters["trendingBotId"].SetValue(botId);
            if (botId == null) return false;
            TrendingBotManager.Instance.StartKeepAlive(botId, Graph);
            return true;
        }
    }
}
