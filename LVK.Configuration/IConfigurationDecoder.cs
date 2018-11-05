using System;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationDecoder
    {
        [NotNull]
        Type SupportedType { get; }

        [NotNull]
        object Decode([NotNull] object value);
    }
}