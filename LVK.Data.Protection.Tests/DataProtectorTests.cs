using System.Text;

using LVK.Core.Services;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Data.Protection.Tests
{
    [TestFixture]
    public class DataProtectorTests
    {
        [Test]
        [TestCase("This is a test of the protection system")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ProtectUnprotect_WithTestCases_ProducesCorrectResults(string testcase)
        {
            const string password = "This is a dummy password";

            var protector = new DataEncryption(new DataEncoder());

            var input = Encoding.UTF8.GetBytes(testcase);
            var protectedData = protector.Protect(input, password);
            var unprotectedData = protector.Unprotect(protectedData, password);
            var output = Encoding.UTF8.GetString(unprotectedData);

            Assert.That(output, Is.EqualTo(input));
        }
    }
}