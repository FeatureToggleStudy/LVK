using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration;

using Newtonsoft.Json.Linq;

namespace LVK.Net.Http.Server.Configuration
{
    internal class ConfigurationConfiguratorTarget : IConfigurationBuilder
    {
        public IConfiguration Build() => throw new NotSupportedException();

        [NotNull, ItemNotNull]
        public List<JsonConfigurationFile> JsonFiles { get; } = new List<JsonConfigurationFile>();

        public void AddJsonFile(string filename, Encoding encoding = null, bool isOptional = false)
        {
            JsonFiles.Add(new JsonConfigurationFile(filename, isOptional));
        }

        public void AddJson(string json)
        {
            
        }

        public void AddDynamic(Func<JObject> getConfiguration)
        {
            
        }

        public void AddEnvironmentVariables(string prefix)
        {
            
        }

        public void AddCommandLine(string[] args)
        {
            
        }
    }
}