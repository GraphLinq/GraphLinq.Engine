using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeGraphDescription : Attribute
    {
        public string Description { get; }

        public NodeGraphDescription(string description)
        {
            Description = description;
        }
    }
}
