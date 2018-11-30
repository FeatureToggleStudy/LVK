using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    internal abstract class PidFileBackgroundServiceBase
    {
        [NotNull, ItemNotNull]
        private readonly Lazy<string[]> _PidFilePaths;

        protected PidFileBackgroundServiceBase()
        {
            _PidFilePaths = new Lazy<string[]>(() => GetPidFilePaths().ToArray());
        }

        [NotNull]
        protected string[] PidFilePaths => _PidFilePaths.Value;

        [NotNull, ItemNotNull]
        private IEnumerable<string> GetPidFilePaths()
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            yield return assembly.Location + ".pid";

            var assemblyNameWithExtension = Path.GetFileName(assembly.Location);
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyNameWithExtension);
            yield return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyName, assemblyNameWithExtension + ".pid");
        }
    }
}