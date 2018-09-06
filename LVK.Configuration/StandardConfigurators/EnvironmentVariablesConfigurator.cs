using System.IO;
using System.Reflection;

using LVK.Core;

namespace LVK.Configuration.StandardConfigurators
{
    internal class EnvironmentVariablesConfigurator : IConfigurationConfigurator
    {
        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            var entryAssemblyLocation = assembly.NotNull().Location.NotNull();
            var entryAssemblyFilename = Path.GetFileNameWithoutExtension(entryAssemblyLocation);
                        
            configurationBuilder.AddEnvironmentVariables($"{entryAssemblyFilename.ToUpper()}_");
        }
    }
}