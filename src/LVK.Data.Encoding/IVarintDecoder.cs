using System;

using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public interface IVarintDecoder
    {
        (ushort value, int bytesConsumed) DecodeUInt16(Span<byte> source);
        (short value, int bytesConsumed) DecodeInt16(Span<byte> source);
        (uint value, int bytesConsumed) DecodeUInt32(Span<byte> source);
        (int value, int bytesConsumed) DecodeInt32(Span<byte> source);
        (ulong value, int bytesConsumed) DecodeUInt64(Span<byte> source);
        (long value, int bytesConsumed) DecodeInt64(Span<byte> source);
    }
}