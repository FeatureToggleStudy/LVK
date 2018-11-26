using System.Collections.Generic;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Extensibility.Plugins
{
    internal class PluginConfiguration
    {
        public PluginConfiguration()
        {
            DirectoryPaths.AddRange(GetDefaultPaths());
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<string> GetDefaultPaths()
        {
            yield return Path.Combine(GetAssemblyLocation(), "Plugins");
        }
        
        [NotNull]
        private static string GetAssemblyLocation()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            return Path.GetDirectoryName(assembly.Location).NotNull();
        }

        [NotNull, ItemNotNull]
        public List<string> DirectoryPaths { get; } = new List<string>();
    }
}