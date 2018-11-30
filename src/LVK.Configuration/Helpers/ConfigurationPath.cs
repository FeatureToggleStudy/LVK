using System;

using JetBrains.Annotations;

namespace LVK.Configuration.Helpers
{
    internal static class ConfigurationPath
    {
        [NotNull]
        public static string Combine([NotNull] string path, [NotNull, ItemNotNull] string[] relativePaths)
        {
            foreach (var relativePath in relativePaths)
                path = Combine(path, relativePath);

            return path;
        }

        [NotNull]
        public static string Combine([NotNull] string path, [NotNull] string relativePath)
        {
            if (relativePath.StartsWith("/"))
                throw new ArgumentException("Only relative configuration paths allowed", nameof(relativePath));

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(relativePath))
                return path + relativePath;

            if (path.EndsWith("/"))
                return path + relativePath;

            return path + "/" + relativePath;
        }
    }
}