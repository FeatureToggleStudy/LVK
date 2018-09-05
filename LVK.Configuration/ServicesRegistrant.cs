using System;
using System.IO;
using System.Linq;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Configuration
{
    [PublicAPI]
    public class ServicesRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
                throw new ArgumentNullException(nameof(containerBuilder));
        }

        public void Register(IContainer container)
        {
            if (container is null)
                throw new ArgumentNullException(nameof(container));

            var configurationBuilder = new ConfigurationBuilder();
            
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            assume(assembly != null);
            
            var entryAssemblyLocation = assembly.NotNull().Location.NotNull();
            var entryAssemblyFilename = Path.GetFileNameWithoutExtension(entryAssemblyLocation);
            var entryAssemblyDirectory = Path.GetDirectoryName(entryAssemblyLocation).NotNull();
            
            configurationBuilder.SetBasePath(entryAssemblyDirectory);
            
            configurationBuilder.AddJsonFile("appsettings.json", isOptional: true);
            configurationBuilder.AddJsonFile("appsettings.debug.json", isOptional: true);

            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            configurationBuilder.AddCommandLine(args);
            
            configurationBuilder.AddEnvironmentVariables($"{entryAssemblyFilename.ToUpper()}_");

            container.UseInstance(configurationBuilder.Build());
        }
    }
}