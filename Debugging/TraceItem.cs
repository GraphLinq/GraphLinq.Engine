using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Debugging
{
    public class TraceItem
    {
        [JsonIgnore]
        public Node Node { get; }

        private string NodeId;
        private Dictionary<string, string> Parameters;
        public long ExecutionTime;
        public Exception ExecutionException;

        public TraceItem(Node node)
        {
            this.Node = node;

            this.Parameters = new Dictionary<string, string>();
            this.NodeId = this.Node.Id;
            foreach(var p in this.Node.InParameters)
            {
                if(p.Value.GetValue() == null)
                {
                    Parameters.Add(p.Key, "NULL");
                }
                else
                {
                    Parameters.Add(p.Key, p.Value.GetValue().ToString());
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("at " + this.Node.NodeType + " " + this.ExecutionTime + "ms (" + this.NodeId + ")");
            if(this.ExecutionException != null)
            {
                sb.AppendLine("Exception occured : " + this.ExecutionException.Message);
            }
            foreach (var p in this.Parameters)
            {
                sb.AppendLine("-- " + p.Key + " = " + p.Value);
            }
            return sb.ToString();
        }
    }
}
