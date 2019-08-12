using System;

namespace LVK.Data.Encoding
{
    internal class VarintCodec : IVarintCodec
    {
        public int Encode(ushort value, Span<byte> target) => throw new NotImplementedException();

        public int Encode(short value, Span<byte> target) => throw new NotImplementedException();

        public int Encode(uint value, Span<byte> target)
        {
            unchecked
            {
                if (value == 0)
                {
                    target[0] = 0;
                    return 1;
                }

                int index = 0;
                while (value > 0)
                {
                    byte next = (byte)(value & 0x7f);
                    value >>= 7;

                    if (value > 0)
                        next |= 0x80;

                    target[index++] = next;
                }

                return index;
            }
        }

        public int Encode(int value, Span<byte> target) => throw new NotImplementedException();

        public int Encode(ulong value, Span<byte> target) => throw new NotImplementedException();

        public int Encode(long value, Span<byte> target) => throw new NotImplementedException();

        public (ushort value, int bytesConsumed) DecodeUInt16(Span<byte> source)
        {
            unchecked
            {
                int index = 0;
                ushort result = 0;

                bool more = true;
                while (more)
                {
                    byte next = source[index];
                    more = (next & 0x80) != 0;
                    next = (byte)(next & 0x7f);

                    result = (ushort)(result | (next << index * 7));
                    index++;
                }

                return (result, index);
            }
        }

        public (short value, int bytesConsumed) DecodeInt16(Span<byte> source) => throw new NotImplementedException();

        public (uint value, int bytesConsumed) DecodeUInt32(Span<byte> source) => throw new NotImplementedException();

        public (int value, int bytesConsumed) DecodeInt32(Span<byte> source) => throw new NotImplementedException();

        public (ulong value, int bytesConsumed) DecodeUInt64(Span<byte> source) => throw new NotImplementedException();

        public (long value, int bytesConsumed) DecodeInt64(Span<byte> source) => throw new NotImplementedException();
    }
}