using System;
using System.Collections.Generic;
using System.Linq;

using NodaTime;
using NodaTime.Testing;

using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Logging.Tests
{
    [TestFixture]
    public class TextLogFormatterTests
    {
        private IClock _Clock;
        private DateTimeZone _SystemTimeZone;

        [SetUp]
        public void SetUp()
        {
            _Clock = new FakeClock(Instant.FromUtc(2018, 1, 1, 0, 0, 0), Duration.FromSeconds(1));
            _SystemTimeZone = DateTimeZone.ForOffset(Offset.Zero);
        }

        [Test]
        public void Constructor_NullClock_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TextLogFormatter(null, _SystemTimeZone));
        }

        [Test]
        public void Constructor_NullSystemTimeZone_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TextLogFormatter(_Clock, null));
        }

        [Test]
        [TestCase(LogLevel.Debug, "2018-01-01 00:00:00.000 DEBUG: DUMMY")]
        [TestCase(LogLevel.Trace, "2018-01-01 00:00:00.000 TRACE: DUMMY")]
        [TestCase(LogLevel.Verbose, "2018-01-01 00:00:00.000 INFO : DUMMY")]
        [TestCase(LogLevel.Information, "2018-01-01 00:00:00.000 INFO : DUMMY")]
        [TestCase(LogLevel.Warning, "2018-01-01 00:00:00.000 WARN : DUMMY")]
        [TestCase(LogLevel.Error, "2018-01-01 00:00:00.000 ERROR: DUMMY")]
        public void Format_LogLevelExamples_ProducesExpectedOutput(LogLevel logLevel, string expected)
        {
            var formatter = new TextLogFormatter(_Clock, _SystemTimeZone);

            List<string> output = formatter.Format(logLevel, "DUMMY").ToList();

            CollectionAssert.AreEqual(new[] { expected }, output);
        }

        [Test]
        public void Format_MultiLineInput_ProducesExpectedOutput()
        {
            var formatter = new TextLogFormatter(_Clock, _SystemTimeZone);

            var input = "Line 1" + Environment.NewLine + "Line 2";
            List<string> output = formatter.Format(LogLevel.Debug, input).ToList();

            CollectionAssert.AreEqual(
                new[] { "2018-01-01 00:00:00.000 DEBUG: Line 1", "2018-01-01 00:00:01.000 DEBUG: Line 2" }, output);
        }
    }
}