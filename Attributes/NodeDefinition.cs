using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeDefinition : Attribute
    {
        public NodeDefinition(string nodeName, string friendlyName, string nodeType, string groupName, int blockLimitPerGraph = -1)
        {
            NodeName = nodeName;
            FriendlyName = friendlyName;
            NodeType = nodeType;
            GroupName = groupName;
            BlockLimitPerGraph = blockLimitPerGraph;
        }

        public string NodeName { get; }
        public string FriendlyName { get; }
        public string NodeType { get; }
        public string GroupName { get; }
        public int BlockLimitPerGraph { get; }
        public string CustomIcon { get; set; }
    }
}
