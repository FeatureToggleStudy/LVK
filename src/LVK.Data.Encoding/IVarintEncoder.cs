using System;

using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public interface IVarintEncoder
    {
        int Encode(ushort value, Span<byte> target);
        int Encode(short value, Span<byte> target);
        int Encode(uint value, Span<byte> target);
        int Encode(int value, Span<byte> target);
        int Encode(ulong value, Span<byte> target);
        int Encode(long value, Span<byte> target);
    }
}