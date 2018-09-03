using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;

using Newtonsoft.Json;
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

        public void AddJsonFile([NotNull] string filename, [CanBeNull] Encoding encoding = null,
                                bool isOptional = false)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            var fullPath = GetFullPath(filename);
            if (!File.Exists(fullPath))
            {
                if (isOptional)
                    return;

                throw new InvalidOperationException();
            }

            AddJson(File.ReadAllText(fullPath, encoding ?? Encoding.UTF8));
        }

        private string GetFullPath(string filename)
        {
            if (Path.IsPathRooted(filename))
                return filename;
            
            return Path.Combine(_BasePath, filename);
        }

        public void AddJson([NotNull] string json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            JObject additional = JObject.Parse(json);
            if (additional == null)
                return;

            Apply(additional, _Root);
        }

        [NotNull]
        public IConfiguration Build() => new Configuration(_Root.DeepClone().NotNull());

        private void Apply([NotNull] JObject source, [NotNull] JObject target)
        {
            var child = source.First;
            while (child != null)
            {
                if (child is JProperty prop)
                    OverrideProperty(target, prop);
                else
                    throw new InvalidOperationException($"unknown: '{child.GetType().Name}'");

                child = child.Next;
            }
        }

        private void OverrideProperty([NotNull] JObject target, [NotNull] JProperty prop)
        {
            assume(prop.Name != null);

            if (!target.ContainsKey(prop.Name))
            {
                target[prop.Name] = prop.Value;
                return;
            }

            assume(target[prop.Name] != null);
            if (prop.Value?.GetType() != target[prop.Name].GetType())
                target[prop.Name] = prop.Value;
            else
                OverrideValue(target, prop);
        }

        private void OverrideValue([NotNull] JObject target, [NotNull] JProperty prop)
        {
            assume(prop.Name != null);

            switch (prop.Value)
            {
                case null:
                    break;

                case JValue val:
                    target[prop.Name] = val;
                    break;

                case JArray a:
                    target[prop.Name] = a;
                    break;

                case JObject obj:
                    Apply(obj, (JObject)target[prop.Name].NotNull());
                    break;

                default:
                    throw new InvalidOperationException($"unknown value-type: '{prop.Value.GetType().Name}'");
            }
        }

        public void AddCommandLine([NotNull, ItemNotNull] string[] args)
        {
            var reExplicitValue = new Regex("--(?<path>[a-z_][a-z0-9_/]*)=(?<value>.*)", RegexOptions.IgnoreCase);
            var reJsonFile = new Regex("@(?<filename>.*)");
            foreach (var arg in args)
            {
                Match ma = reExplicitValue.Match(arg);
                if (ma.Success)
                {
                    var path = ma.Groups["path"]?.Value ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(path))
                        continue;

                    JToken value = ValueFromString(ma.Groups["value"]?.Value ?? string.Empty);
                    Apply(Construct(path.Split('/'), value), _Root);
                    continue;
                }

                ma = reJsonFile.Match(arg);
                if (ma.Success)
                {
                    var filename = ma.Groups["filename"]?.Value ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(filename))
                        continue;
                    
                    AddJsonFile(filename);
                }
            }
        }

        private JToken ValueFromString(string value)
        {
            try
            {
                return JToken.Parse(value);
            }
            catch (JsonReaderException)
            {
                return new JValue(value);
            }
        }

        [NotNull]
        private JObject Construct([NotNull, ItemNotNull] string[] path, [NotNull] JToken value)
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
                var value = ValueFromString(environmentVariables[key]?.ToString() ?? string.Empty);

                Apply(Construct(path.Split('/'), value), _Root);
            }
        }
    }
}