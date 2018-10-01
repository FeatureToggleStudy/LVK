using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Configuration.StandardConfigurators
{
    internal class AppSettingsConfigurator : IConfigurationConfigurator
    {
        [NotNull, ItemNotNull]
        private IEnumerable<string[]> GetCombinations()
        {
            yield return new string[0];
            yield return new[] { Environment.MachineName };
            yield return new[] { Environment.UserName };
            yield return new[] { Environment.MachineName, Environment.UserName };
        }

        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            foreach (string[] combination in GetCombinations())
            {
                string filename = string.Join(".", new[] { "appsettings" }.Concat(combination));

                configurationBuilder.AddJsonFile("hosting.json", isOptional: true);
                configurationBuilder.AddJsonFile(filename + ".json", isOptional: true);
                configurationBuilder.AddJsonFile(filename + ".debug.json", isOptional: true);
            }
        }
    }
}