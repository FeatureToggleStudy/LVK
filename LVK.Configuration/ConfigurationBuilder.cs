using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    internal class ConfigurationBuilder
    {
        [NotNull]
        private string _BasePath;

        [NotNull]
        private readonly JObject _Root = new JObject();

        public ConfigurationBuilder()
        {
            _BasePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().NotNull().Location).NotNull();
        }

        public void SetBasePath([NotNull] string basePath)
        {
            _BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }

        public void AddJsonFile([NotNull] string filename, [CanBeNull] Encoding encoding = null, bool isOptional = false)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            var fullPath = Path.Combine(_BasePath, filename);
            if (!File.Exists(fullPath))
            {
                if (isOptional)
                    return;

                throw new InvalidOperationException();
            }

            JObject additional = JObject.Parse(File.ReadAllText(fullPath, encoding ?? Encoding.UTF8));
            if (additional == null)
                return;

            Apply(additional, _Root);
        }

        public IConfiguration Build() => new Configuration(_Root.DeepClone().NotNull());

        private void Apply([NotNull] JObject source, [NotNull] JObject target)
        {
            var child = source.First;
            while (child != null)
            {
                switch (child)
                {
                    case JProperty prop:
                        assume(prop.Name != null);
                        
                        if (!target.ContainsKey(prop.Name))
                            target[prop.Name] = prop.Value;
                        else
                        {
                            assume(target[prop.Name] != null);
                            if (prop.Value?.GetType() != target[prop.Name].GetType())
                                target[prop.Name] = prop.Value;
                            else
                            {
                                switch (prop.Value)
                                {
                                    case JValue val:
                                        target[prop.Name] = val;
                                        break;

                                    case JArray a:
                                        target[prop.Name] = a;
                                        break;

                                    case JObject obj:
                                        Apply(obj, (JObject)target[prop.Name]);
                                        break;

                                    default:
                                        throw new InvalidOperationException(
                                            $"unknown value-type: '{prop.Value.GetType().Name}'");
                                }
                            }
                        }

                        break;

                    default:
                        throw new InvalidOperationException($"unknown: '{child.GetType().Name}'");
                }

                child = child.Next;
            }
        }

        public void AddCommandLine([NotNull, ItemNotNull] string[] args)
        {
            var re = new Regex("--(?<path>[a-z_][a-z0-9_/]*)=(?<value>.*)", RegexOptions.IgnoreCase);
            foreach (var arg in args)
            {
                Match ma = re.Match(arg);
                if (!ma.Success)
                    continue;

                var path = ma.Groups["path"]?.Value ?? string.Empty;
                if (string.IsNullOrWhiteSpace(path))
                    continue;

                var value = ma.Groups["value"]?.Value ?? string.Empty;

                Apply(Construct(path.Split('/'), value), _Root);
            }
        }

        [NotNull]
        private JObject Construct([NotNull, ItemNotNull] string[] path, [NotNull] string value)
        {
            var root = new JObject();
            var current = root;
            for (int index = 0; index < path.Length - 1; index++)
            {
                var child = new JObject();
                current[path[index]] = child;
                current = child;
            }

            current[path[path.Length - 1]] = value;
            return root;
        }

        public void AddEnvironmentVariables([NotNull] string prefix)
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (string key in environmentVariables.Keys)
            {
                if (!key.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var path = key.Substring(prefix.Length);
                string value = environmentVariables[key]?.ToString() ?? string.Empty;
                
                Apply(Construct(path.Split('/'), value), _Root);
            }
        }
    }
}