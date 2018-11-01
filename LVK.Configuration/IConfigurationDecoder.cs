using System;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationDecoder
    {
        [NotNull]
        object Decode([NotNull] object value);

        bool CanDecode([NotNull] Type type);
    }
}