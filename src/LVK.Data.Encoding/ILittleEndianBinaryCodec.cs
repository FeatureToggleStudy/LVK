using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public interface ILittleEndianBinaryCodec : ILittleEndianBinaryEncoder, ILittleEndianBinaryDecoder
    {
    }
}