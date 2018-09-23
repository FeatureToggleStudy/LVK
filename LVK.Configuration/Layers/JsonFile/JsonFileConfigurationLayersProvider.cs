using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class JsonFileConfigurationLayersProvider : IConfigurationLayersProvider
    {
        [NotNull]
        private readonly string _Filename;

        [NotNull]
        private readonly Encoding _Encoding;

        private readonly bool _IsOptional;

        public JsonFileConfigurationLayersProvider([NotNull] string filename, [CanBeNull] Encoding encoding, bool isOptional)
        {
            _Filename = filename ?? throw new ArgumentNullException(nameof(filename));
            _Encoding = encoding ?? Encoding.UTF8;
            _IsOptional = isOptional;
        }

        public IEnumerable<IConfigurationLayer> Provide()
        {
            yield return new JsonFileConfigurationLayer(_Filename, _Encoding, _IsOptional);
        }
    }
}