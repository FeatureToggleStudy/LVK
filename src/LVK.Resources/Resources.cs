using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

using JetBrains.Annotations;

using LVK.Json;
using LVK.Logging;

using Newtonsoft.Json;

namespace LVK.Resources
{
    internal class Resources : IResources
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IJsonSerializerFactory _JsonSerializerFactory;

        [NotNull]
        private readonly Assembly _Assembly;

        [NotNull]
        private readonly string _NamePrefix;

        public Resources([NotNull] ILogger logger, [NotNull] IJsonSerializerFactory jsonSerializerFactory, [NotNull] Assembly assembly, [NotNull] string namePrefix)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _JsonSerializerFactory = jsonSerializerFactory ?? throw new ArgumentNullException(nameof(jsonSerializerFactory));
            _Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _NamePrefix = namePrefix ?? throw new ArgumentNullException(nameof(namePrefix));
        }

        public Stream OpenBinary(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            using (_Logger.LogScope(LogLevel.Debug, $"Opening resource '{_NamePrefix}{name}' in '{_Assembly}'"))
            {
                var stream = _Assembly.GetManifestResourceStream(_NamePrefix + name);
                if (stream is null)
                    throw new MissingManifestResourceException($"Resource '{name}' is not found");

                return stream;
            }
        }

        public TextReader OpenText(string name, Encoding encoding = null)
            => new StreamReader(OpenBinary(name), encoding ?? Encoding.Default);

        public object DeserializeJson(string name, Type type)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            using (var textReader = OpenText(name, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
                return _JsonSerializerFactory.Create().Deserialize(jsonReader, type);
        }
    }
}