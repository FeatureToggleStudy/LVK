using System;
using System.IO;
using System.Linq;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;


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
            var entryAssemblyLocation = Assembly.GetEntryAssembly().NotNull().Location.NotNull();
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