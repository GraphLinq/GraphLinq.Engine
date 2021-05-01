using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeTimeout : Attribute
    {
        public NodeTimeout(int customTimeout)
        {
            this.CustomTimeout = customTimeout;
        }

        public int CustomTimeout { get; }
    }
}
