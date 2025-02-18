using NodeBlock.Engine.Nodes.Dextools;
using System;

namespace NodeBlock.Engine.Nodes.Bot
{
    public class TrendingBotManager : BotManagerBase
    {
        private static readonly Lazy<TrendingBotManager> _instance = new Lazy<TrendingBotManager>(() => new TrendingBotManager());
        public static TrendingBotManager Instance => _instance.Value;

        private TrendingBotManager() : base("sniperbot_api_base_url") { }

        public string StartBot(string privateKey, double amountEthToSwapForOneToken, double maxTokensAmount, string dextoolsApiKey, BlockGraph graph)
        {
            var payload = new { privateKey, amountEthToSwapForOneToken, maxTokensAmount, dextoolsApiKey };
            return base.StartBot(payload, graph);
        }
    }
}
