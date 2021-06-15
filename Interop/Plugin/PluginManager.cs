using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Engine.Interop.Plugin
{
    public class PluginManager
    {
        private static bool _pluginLoaded = false;
        private static List<BasePlugin> _plugins = new List<BasePlugin>();
        private static List<ExportableObject> _exportableObjects = new List<ExportableObject>();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        
        public static BasePlugin FetchPluginByName(string name)
        {
            return _plugins.FirstOrDefault(x => x.GetType().FullName.Contains(name));
        }

        public static void LoadPlugins()
        {
            if (_pluginLoaded) return;
            if(!Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins")))
            {
                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins"));
            }

            foreach(var pluginDll in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins")))
            {
                try
                {
                    var assembly = Assembly.LoadFile(pluginDll);
                    var basePluginType = assembly.GetTypes().ToList().FirstOrDefault(x => x.IsSubclassOf(typeof(BasePlugin)));
                    if (basePluginType == null) continue;
                    var plugin = Activator.CreateInstance(basePluginType) as BasePlugin;
                    plugin.Load();

                    var nodeCount = 0;
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttributes(typeof(Attributes.NodeDefinition), true).Length == 0) continue;
                        var instance = Activator.CreateInstance(type, string.Empty, null) as Node;
                        NodeBlockExporter.AddNodeType(instance);
                        nodeCount++;
                    }

                    _plugins.Add(plugin);
                    logger.Info("Plugin " + plugin.GetType().FullName + " loaded with " + nodeCount + " nodes and " + _exportableObjects.Count + " exportables objects");
                }
                catch(Exception ex)
                {
                    logger.Error(ex, "Can't load the plugin : " + pluginDll);
                }
            }

            _pluginLoaded = true;
        }
    }
}
