using NodeBlock.Engine.Nodes.Dextools;
using System;

namespace NodeBlock.Engine.Nodes.Bot
{
    public class ArbitrageBotManager : BotManagerBase
    {
        private static readonly Lazy<ArbitrageBotManager> _instance = new Lazy<ArbitrageBotManager>(() => new ArbitrageBotManager());
        public static ArbitrageBotManager Instance => _instance.Value;

        private ArbitrageBotManager() : base("arbitrage_api_base_url") { }

        public string StartBot(string privateKey, double amountEthToSwap, double amountGlqToSwap, int secondsToRetryAttempt, BlockGraph graph)
        {
            var payload = new { privateKey, amountEthToSwap, amountGlqToSwap, secondsToRetryAttempt };
            return base.StartBot(payload, graph);
        }
    }
}
