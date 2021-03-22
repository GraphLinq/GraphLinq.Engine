using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Engine.Debugging
{
    public class GraphTrace
    {
        [JsonIgnore]
        public GraphExecutionCycle Cycle { get; }

        public List<TraceItem> Stack;
        public Exception Exception;

        public GraphTrace(GraphExecutionCycle cycle)
        {
            Cycle = cycle;
            this.Stack = new List<TraceItem>();
        }

        public TraceItem AppendTrace(Node node)
        {
            var item = new TraceItem(node);
            this.Stack.Add(item);
            return item;
        }

        public void SetException(Exception exception)
        {
            this.Exception = exception;
        }

        public override string ToString()
        {
            return "Total execution time : " + this.Stack.Select(x => x.ExecutionTime).Sum() + "ms\n" +
                "Execution success : " + (this.Stack.FindAll(x => x.ExecutionException != null).Count > 0 ? "FALSE" : "TRUE") + "\n" + 
                string.Join(string.Empty, this.Stack.AsEnumerable().Reverse()) + "\n"
                + "Stack count: " + this.Stack.Count;
        }
    }
}
