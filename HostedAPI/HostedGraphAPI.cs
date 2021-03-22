using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.HostedAPI
{
    public class HostedGraphAPI
    {
        public BlockGraph Graph { get; set; }
        public Dictionary<string, HostedEndpoint> Endpoints = new Dictionary<string, HostedEndpoint>();

        public HostedGraphAPI(BlockGraph graph)
        {
            this.Graph = graph;
        }

        public string URL
        {
            get
            {
                return Environment.GetEnvironmentVariable("hosted_api_base_url") + "/api/" + Graph.UniqueHash;
            }
        }
    }
}
