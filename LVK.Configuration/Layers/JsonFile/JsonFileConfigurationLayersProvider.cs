using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using NodaTime;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class JsonFileConfigurationLayersProvider : IConfigurationLayersProvider
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly string _Filename;

        [NotNull]
        private readonly Encoding _Encoding;

        private readonly bool _IsOptional;

        public JsonFileConfigurationLayersProvider([NotNull] IClock clock, [NotNull] string filename, [CanBeNull] Encoding encoding, bool isOptional)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _Filename = filename ?? throw new ArgumentNullException(nameof(filename));
            _Encoding = encoding ?? Encoding.UTF8;
            _IsOptional = isOptional;
        }

        public IEnumerable<IConfigurationLayer> Provide()
        {
            if (_IsOptional)
                yield return new OptionalJsonFileConfigurationLayer(_Clock, _Filename, _Encoding);
            else
                yield return new RequiredJsonFileConfigurationLayer(_Clock, _Filename, _Encoding);
        }
    }
}