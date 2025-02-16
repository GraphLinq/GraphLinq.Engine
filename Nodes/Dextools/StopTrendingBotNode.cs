using NodeBlock.Engine.Attributes;
using NodeBlock.Engine.Nodes.Bot;

namespace NodeBlock.Engine.Nodes.Dextools
{
    [NodeDefinition("StopTrendingBotNode", "Stop Trending Bot", NodeTypeEnum.Function, "Dextools")]
    [NodeGraphDescription("Stop the trending bot")]
    public class StopTrendingBotNode : Node
    {

        public StopTrendingBotNode(string id, BlockGraph graph)
             : base(id, graph, typeof(StopTrendingBotNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("trendingBotId", new NodeParameter(this, "trendingBotId", typeof(string), true));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var trendingBotId = this.InParameters["trendingBotId"].GetValue().ToString();

            if (string.IsNullOrEmpty(trendingBotId))
            {
                this.Graph.AppendLog("error", "Trending bot id is empty");
                return false;
            }

            TrendingBotManager.Instance.StopBot(trendingBotId, Graph);
            this.Graph.AppendLog("info", $"Trending bot {trendingBotId} stopped");
            return true;
        }
    }
}
