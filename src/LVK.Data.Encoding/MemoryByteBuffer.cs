using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public class MemoryByteBuffer : ByteBuffer
    {
        private readonly LinkedList<byte[]> _Buffers = new LinkedList<byte[]>();
        
        public MemoryByteBuffer()
            : base(65536)
        {
        }

        protected override void AcceptBuffer(byte[] buffer, int inBuffer)
        {
            var temp = new byte[inBuffer];
            Array.Copy(buffer, temp, inBuffer);
            _Buffers.AddLast(temp);
        }

        public void Clear()
        {
            _Buffers.Clear();
        }

        [NotNull]
        public byte[] ToArray()
        {
            var length = _Buffers.Sum(buffer => buffer.Length);
            var result = new byte[length];
            var index = 0;

            foreach (byte[] buffer in _Buffers)
            {
                Array.Copy(buffer, 0, result, index, buffer.Length);
                index += buffer.Length;
            }

            return result;
        }
    }
}