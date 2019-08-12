using System;

using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public abstract class ByteBuffer : IDisposable
    {
        private const int DefaultBufferSize = 65536;
        
        private byte[] _Buffer;
        private int _InBuffer;

        protected ByteBuffer(int bufferSize)
        {
            if (bufferSize < 256)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), $"{nameof(bufferSize)} must be 256 or higher");
            
            _Buffer = new byte[bufferSize];
        }

        public void Append(byte value)
        {
            _Buffer[_InBuffer++] = value;
            if (_InBuffer == _Buffer.Length)
                Flush();
        }

        public void Append([NotNull] byte[] values) => Append(values.AsSpan());
        public void Append([NotNull] byte[] values, int offset, int count) => Append(values.AsSpan().Slice(offset, count));
        public void Append(Span<byte> values)
        {
            foreach (var value in values)
                Append(value);
        }
        
        public void Flush()
        {
            if (_InBuffer == 0)
                return;

            AcceptBuffer(_Buffer, _InBuffer);
            _InBuffer = 0;
        }

        protected abstract void AcceptBuffer([NotNull] byte[] buffer, int inBuffer);

        protected virtual void Dispose(bool disposing)
        {
            Flush();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}