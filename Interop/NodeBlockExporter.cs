using Newtonsoft.Json;
using NodeBlock.Engine.Nodes;
using NodeBlock.Engine.Nodes.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace NodeBlock.Engine.Interop
{
    public static class NodeBlockExporter
    {
        private static List<Node> nodes = new List<Node>();

        public static void ExportNodesSchema(string filePath)
        {
            GraphManager.InitGraphEngine();
            Plugin.PluginManager.LoadPlugins();
            File.WriteAllText(@filePath, JsonConvert.SerializeObject(nodes));
        }

        public static void ShowNodesInfos()
        {
            nodes.Where(x => x.NodeBlockType == "condition").ToList().ForEach(x =>
            {
                Console.WriteLine(string.Format("```\n\"Block Type\" -> \"{0}\"\n\"Description\" -> \"{1}\"\n\"Block Gas Cost\" -> 0\n```", x.GetType().ToString(), x.NodeDescription));
            });
        }

        public static void AddNodeType(Node node)
        {
            nodes.Add(node);
        }

        public static List<Node> GetNodes()
        {
            return nodes;
        }
    }
}
