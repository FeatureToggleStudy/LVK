using System;
using System.Collections;
using System.IO;
using System.Linq;
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
    internal class ConfigurationBuilder : IConfigurationBuilder
    {
        [NotNull]
        private string _BasePath;

        [NotNull]
        private readonly JObject _Root = new JObject();

        public ConfigurationBuilder()
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            
            _BasePath = Path.GetDirectoryName(assembly.Location).NotNull();
        }

        public void AddJsonFile(string filename, Encoding encoding = null,
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

        [NotNull]
        private string GetFullPath([NotNull] string filename)
        {
            if (Path.IsPathRooted(filename))
                return filename;
            
            return Path.Combine(_BasePath, filename);
        }

        public void AddJson(string json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            JObject additional = JObject.Parse(json);
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

        public void AddCommandLine(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Apply(new JObject { ["CommandLineArguments"] = new JArray(args.OfType<object>().ToArray()) }, _Root);
            
            var reExplicitValue = new Regex("--(?<path>[a-z_][a-z0-9_/]*)(=(?<value>.*))?", RegexOptions.IgnoreCase);
            var reJsonFile = new Regex("@(?<filename>.*)");
            foreach (var arg in args)
            {
                Match ma = reExplicitValue.Match(arg);
                if (ma.Success)
                {
                    ApplyCommandLineValueOverride(ma);
                    continue;
                }

                ma = reJsonFile.Match(arg);
                if (ma.Success)
                    ApplyCommandLineOptionsFileOverride(ma);
            }
        }

        private void ApplyCommandLineOptionsFileOverride([NotNull] Match match)
        {
            var filename = match.Groups["filename"]?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(filename))
                return;

            AddJsonFile(filename);
        }

        private void ApplyCommandLineValueOverride([NotNull] Match match)
        {
            var path = match.Groups["path"]?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(path))
                return;

            var group = match.Groups["value"];
            var stringValue = group.Success ? group.Value : "true";
            JToken value = ValueFromString(stringValue);
            Apply(Construct(path.Split('/'), value), _Root);
        }

        [NotNull]
        private JToken ValueFromString(string value)
        {
            try
            {
                return JToken.Parse(value) ?? new JValue(string.Empty);
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

        public void AddEnvironmentVariables(string prefix)
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