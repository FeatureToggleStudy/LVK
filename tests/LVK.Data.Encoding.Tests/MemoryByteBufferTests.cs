using System.Linq;

using NUnit.Framework;

namespace LVK.Data.Encoding.Tests
{
    [TestFixture]
    public class MemoryByteBufferTests
    {
        [Test]
        public void ToArray_EmptyBuffer_ReturnsEmptyArray()
        {
            var buffer = new MemoryByteBuffer();

            byte[] output = buffer.ToArray();

            CollectionAssert.IsEmpty(output);
        }

        [Test]
        public void ToArray_AfterWritingBytesToBufferAndFlushing_ReturnsArrayWithBytes()
        {
            var buffer = new MemoryByteBuffer();

            for (int index = 0; index < 256; index++)
                buffer.Append((byte)index);

            buffer.Flush();
            byte[] output = buffer.ToArray();

            CollectionAssert.AreEqual(Enumerable.Range(0, 256), output);
        }
    }
}