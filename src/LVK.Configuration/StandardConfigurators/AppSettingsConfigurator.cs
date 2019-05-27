using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Configuration.StandardConfigurators
{
    internal class AppSettingsConfigurator : IConfigurationConfigurator
    {
        [NotNull, ItemNotNull]
        private IEnumerable<string[]> GetCombinations()
        {
            yield return new string[0];
            
            yield return new[] { Environment.UserDomainName };
            yield return new[] { "domain", Environment.UserDomainName };
            
            if (Environment.UserDomainName != Environment.MachineName)
                yield return new[] { Environment.MachineName };
            yield return new[] { "machine", Environment.MachineName };
            
            yield return new[] { Environment.UserName };
            yield return new[] { "user", Environment.UserName };
        }

        [CanBeNull]
        private string GetDeploymentEnvironmentName()
        {
            var re = new Regex(@"^--env(ironment)?=(?<env>.*)$", RegexOptions.IgnoreCase);
            string environmentName;
            foreach (string parameter in Environment.GetCommandLineArgs())
            {
                var ma = re.Match(parameter);
                if (!ma.Success)
                    continue;

                environmentName = ma.Groups["env"].NotNull().Value.Trim();
                if (!string.IsNullOrWhiteSpace(environmentName))
                    return environmentName;
            }

            string applicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            if (applicationName == null)
                return null;

            environmentName = Environment.GetEnvironmentVariable($"{applicationName}_ENVIRONMENT")?.Trim();
            if (!string.IsNullOrWhiteSpace(environmentName))
                return environmentName;

            return null;
        }

        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("hosting.json", isOptional: true);
            string environmentName = GetDeploymentEnvironmentName();
            foreach (string[] combination in GetCombinations())
            {
                string filename = string.Join(".", new[] { "appsettings" }.Concat(combination));

                configurationBuilder.AddJsonFile($"{filename}.json", isOptional: true);
                if (environmentName != null)
                    configurationBuilder.AddJsonFile($"{filename}.{environmentName}.json", isOptional: true);
                
                configurationBuilder.AddJsonFile($"{filename}.debug.json", isOptional: true);
                if (environmentName != null)
                    configurationBuilder.AddJsonFile($"{filename}.{environmentName}.debug.json", isOptional: true);
            }
        }
    }
}