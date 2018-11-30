using System;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Configuration.Helpers
{
    internal class ConfigurationFilePath
    {
        [NotNull, ItemNotNull]
        private static readonly Lazy<string> _AssemblyLocation = new Lazy<string>(GetAssemblyLocation);

        [NotNull]
        private static string GetAssemblyLocation()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            return Path.GetDirectoryName(assembly.Location).NotNull();
        }

        [NotNull]
        public static string GetFullPath([NotNull] string relativeFilePath)
        {
            if (Path.IsPathRooted(relativeFilePath))
                return relativeFilePath;

            return Path.Combine(_AssemblyLocation.Value, relativeFilePath);
        }
    }
}