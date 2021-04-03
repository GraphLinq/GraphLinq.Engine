using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Nodes.Functions
{
    public class FunctionContext
    {
        public FunctionContext(FunctionNode node)
        {
            Node = node;
            this.ReturnValues = new Dictionary<string, object>();
        }

        public FunctionNode Node { get; }
        public Dictionary<string, object> ReturnValues { get; set; }
    }
}
