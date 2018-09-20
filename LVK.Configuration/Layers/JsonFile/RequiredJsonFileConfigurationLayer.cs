using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration.Helpers;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class RequiredJsonFileConfigurationLayer : IConfigurationLayer
    {
        [NotNull]
        private readonly string _Filename;

        [NotNull]
        private readonly Encoding _Encoding;

        public RequiredJsonFileConfigurationLayer([NotNull] string filename, [NotNull] Encoding encoding)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            _Filename = ConfigurationFilePath.GetFullPath(filename);
            _Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public Instant LastChangedAt => Instant.FromDateTimeUtc(File.GetLastWriteTimeUtc(_Filename));

        public JObject GetConfiguration() => JObject.Parse(File.ReadAllText(_Filename, _Encoding));
    }
}