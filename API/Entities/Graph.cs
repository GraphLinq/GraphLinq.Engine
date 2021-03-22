using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Engine.Enums;

namespace NodeBlock.Engine.API.Entities
{
    public class Graph
    {
        public string UniqueHash { get; set; }

        public GraphStateEnum DeployedState { get; set; }
        public string RawBytes { get; set; }
        public int WalletIdentifier { get; set; }

        public bool Debug { get; set; }
    }
}
