using JetBrains.Annotations;

namespace LVK.Json
{
    [PublicAPI]
    public interface IJsonStringDecoder
    {
        [NotNull]
        string Decode([NotNull] string value);
    }
}