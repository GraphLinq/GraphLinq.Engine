using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeCycleLimit : Attribute
    {
        public int LimitPerCycle { get; }

        public NodeCycleLimit(int limitPerCycle)
        {
            LimitPerCycle = limitPerCycle;
        }
    }
}
