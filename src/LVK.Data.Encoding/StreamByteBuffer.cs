using System;
using System.IO;

using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    public class StreamByteBuffer : ByteBuffer
    {
        [NotNull]
        private readonly Stream _Target;

        public StreamByteBuffer([NotNull] Stream target, int bufferSize)
            : base(bufferSize)
        {
            _Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        protected override void AcceptBuffer(byte[] buffer, int inBuffer) => _Target.Write(buffer, 0, inBuffer);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                _Target.Dispose();
        }
    }
}