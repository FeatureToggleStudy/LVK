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
        private readonly Lazy<string[]> _PidFilenames;

        protected PidFileBackgroundServiceBase()
        {
            _PidFilenames = new Lazy<string[]>(() => GetPidFilenames().ToArray());
        }

        [NotNull]
        protected string[] PidFilenames => _PidFilenames.Value;

        [NotNull, ItemNotNull]
        private IEnumerable<string> GetPidFilenames()
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