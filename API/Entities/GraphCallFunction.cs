using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.API.Entities
{
    public class GraphCallFunction : Graph
    {
        public string FunctionName { get; set; }
        public Dictionary<string, object> FunctionCallParameters { get; set; }
    }
}
