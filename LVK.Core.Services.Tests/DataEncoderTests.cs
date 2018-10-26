using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class DataEncoderTests
    {
        public static IEnumerable<int> ReadWriteInt32_TestCases()
        {
            for (int index = 0; index <= 30; index++)
            {
                yield return (1 << index) - 1;
                yield return 1 << index;
                yield return (1 << index) + 1;
            }
        }

        [Test]
        [TestCaseSource(nameof(ReadWriteInt32_TestCases))]
        public void ReadWriteInt32_WithTestCases_ProducesCorrectResults(int input)
        {
            var stream = new MemoryStream();
            var encoder = new DataEncoder();

            encoder.WriteInt32(stream, input);
            stream.Position = 0;
            var output = encoder.ReadInt32(stream);

            Assert.That(output, Is.EqualTo(input));
        }
    }
}