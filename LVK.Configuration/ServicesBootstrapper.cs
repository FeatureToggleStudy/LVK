using System;
using System.IO;
using System.Linq;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;

using Microsoft.Extensions.Configuration;

namespace LVK.Configuration
{
    [UsedImplicitly]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var entryAssemblyLocation = Assembly.GetEntryAssembly().NotNull().Location;
            var entryAssemblyFilename = Path.GetFileName(entryAssemblyLocation);
            var entryAssemblyDirectory = Path.GetDirectoryName(entryAssemblyLocation);
            
            configurationBuilder.SetBasePath(entryAssemblyDirectory);
            
            configurationBuilder.AddJsonFile("appsettings.json", optional: true);
            configurationBuilder.AddJsonFile("appsettings.debug.json", optional: true);

            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            configurationBuilder.AddCommandLine(args);
            
            configurationBuilder.AddEnvironmentVariables($"{entryAssemblyFilename.ToUpper()}_");

            IConfigurationRoot configuration = configurationBuilder.Build();
            container.UseInstance<IConfiguration>(configuration);
        }
    }
}