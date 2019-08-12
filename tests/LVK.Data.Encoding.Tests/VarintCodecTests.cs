using NUnit.Framework;

namespace LVK.Data.Encoding.Tests
{
    [TestFixture]
    public class VarintCodecTests
    {
        [Test]
        [TestCase(0u, new byte[] { 0x00 })]
        [TestCase(1u, new byte[] { 0x01 })]
        [TestCase(127u, new byte[] { 0x7f })]
        [TestCase(128u, new byte[] { 0x80, 0x01 })]
        [TestCase(129u, new byte[] { 0x81, 0x01 })]
        [TestCase(16383u, new byte[] { 0xff, 0x7f })]
        [TestCase(16384u, new byte[] { 0x80, 0x80, 0x01 })]
        [TestCase(16385u, new byte[] { 0x81, 0x80, 0x01 })]
        [TestCase(65535u, new byte[] { 0xff, 0xff, 0x03})]
        public void Encode_UInt16_WithTestCases(uint input, byte[] expected)
        {
            var codec = new VarintCodec();

            var target = new byte[expected.Length];
            var encoded = codec.Encode(input, target);

            Assert.That(encoded, Is.EqualTo(expected.Length));
            CollectionAssert.AreEqual(expected, target);
        }

        [Test]
        [TestCase(new byte[] { 0x00 }, 0u)]
        [TestCase(new byte[] { 0x01 }, 1u)]
        [TestCase(new byte[] { 0x7f }, 127u)]
        [TestCase(new byte[] { 0x80, 0x01 }, 128u)]
        [TestCase(new byte[] { 0x81, 0x01 }, 129u)]
        [TestCase(new byte[] { 0xff, 0x7f }, 16383u)]
        [TestCase(new byte[] { 0x80, 0x80, 0x01 }, 16384u)]
        [TestCase(new byte[] { 0x81, 0x80, 0x01 }, 16385u)]
        [TestCase(new byte[] { 0xff, 0xff, 0x03}, 65535u)]
        public void Decode_UInt16_WithTestCases(byte[] input, uint expected)
        {
            var codec = new VarintCodec();

            var (output, bytesConsumed) = codec.DecodeUInt16(input);

            Assert.That(bytesConsumed, Is.EqualTo(input.Length));
            Assert.That(output, Is.EqualTo(expected));
        }
    }
}