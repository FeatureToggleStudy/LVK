using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public interface IBigEndianBinaryCodec : IBigEndianBinaryEncoder, IBigEndianBinaryDecoder
    {
    }
}