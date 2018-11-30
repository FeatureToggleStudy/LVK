using System;
using System.IO;
using System.Linq;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.DryIoc;

namespace LVK.Extensibility.Plugins
{
    internal class PluginContainerFinalizer : IContainerFinalizer
    {
        [NotNull]
        private readonly IConfigurationElementWithDefault<PluginConfiguration> _Configuration;

        public PluginContainerFinalizer([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration["Plugins"].Element<PluginConfiguration>().WithDefault(() => new PluginConfiguration());
        }

        public void Finalize(IContainer container)
        {
            var uniqueDirectoryPaths = _Configuration.Value().DirectoryPaths.Select(Path.GetFullPath).Distinct().ToList();
            foreach (var directoryPath in uniqueDirectoryPaths)
                LoadPluginsFromDirectory(directoryPath, container);
        }

        private void LoadPluginsFromDirectory([NotNull] string directoryPath, [NotNull] IContainer targetContainer)
        {
            if (!Directory.Exists(directoryPath))
                return;
            
            string[] assemblyFilePaths = Directory.GetFiles(directoryPath, "*.dll");

            var bootstrapperInterfaceType = typeof(IServicesBootstrapper);
            foreach (var assemblyFilePath in assemblyFilePaths)
            {
                var assembly = Assembly.LoadFile(assemblyFilePath);

                var bootstrapperTypes =
                    from type in assembly.GetTypes()
                    where !type.IsAbstract
                    where bootstrapperInterfaceType.IsAssignableFrom(type)
                    let ctor = type.GetConstructor(Type.EmptyTypes)
                    where ctor != null
                    select type;

                foreach (var bootstrapperType in bootstrapperTypes)
                {
                    var bootstrapper = Activator.CreateInstance(bootstrapperType) as IServicesBootstrapper;
                    bootstrapper?.Bootstrap(targetContainer);
                }
            }
        }
    }
}