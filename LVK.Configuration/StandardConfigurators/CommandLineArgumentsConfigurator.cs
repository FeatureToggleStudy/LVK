using System;
using System.Linq;

namespace LVK.Configuration.StandardConfigurators
{
    internal class CommandLineArgumentsConfigurator : IConfigurationConfigurator
    {
        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            configurationBuilder.AddCommandLine(args);
        }
    }
}