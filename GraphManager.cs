using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NodeBlock.Engine
{
    public static class GraphManager
    {
        private static bool _initialized = false;

        public static void InitGraphEngine()
        {
            if (_initialized) return;

            foreach(var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetCustomAttributes(typeof(Attributes.NodeDefinition), true).Length == 0) continue;
                var instance = Activator.CreateInstance(type, string.Empty, null) as Node;
                Interop.NodeBlockExporter.AddNodeType(instance);
            }

            _initialized = true;
        }
    }
}
