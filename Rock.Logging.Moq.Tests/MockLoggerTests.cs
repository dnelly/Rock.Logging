using System.Linq;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Rock.Logging.Moq.Tests
{
    public class MockLoggerTests
    {
        private IEnumerable<LogLevel> _logLevels;

        [SetUp]
        public void Setup()
        {
            _logLevels = new[]
            {
                LogLevel.Debug,
                LogLevel.Info,
                LogLevel.Warn,
                LogLevel.Error,
                LogLevel.Fatal,
                LogLevel.Audit
            };
        }

        [Test]
        public void IsEnabledIsConfiguredCorrectlyForDefaultConstructor()
        {
            var mockLogger = new MockLogger();

            foreach (var level in _logLevels)
            {
                Assert.That(mockLogger.Object.IsEnabled(level), Is.True);
            }

            foreach (var level in _logLevels)
            {
                mockLogger.Verify(logger => logger.IsEnabled(It.Is<LogLevel>(x => x == level)), Times.Once());
            }
        }

        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Info)]
        [TestCase(LogLevel.Warn)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Fatal)]
        [TestCase(LogLevel.Audit)]
        public void IsEnabledIsConfiguredCorrectlyForNonDefaultConstructor(LogLevel logLevel)
        {
            var mockLogger = new MockLogger(logLevel);

            var lowerLevels = _logLevels.Where(x => x < logLevel);
            var higherLevels = _logLevels.Where(x => x >= logLevel);

            foreach (var lowerLevel in lowerLevels)
            {
                Assert.That(mockLogger.Object.IsEnabled(lowerLevel), Is.False);
            }

            foreach (var higherLevel in higherLevels)
            {
                Assert.That(mockLogger.Object.IsEnabled(higherLevel), Is.True);
            }

            foreach (var level in _logLevels)
            {
                mockLogger.Verify(logger => logger.IsEnabled(It.Is<LogLevel>(x => x == level)), Times.Once());
            }
        }

        [Test]
        public void VerifyLog1FailsWithZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            Assert.That(mockLogger.VerifyLog, Throws.InstanceOf<MockException>());
        }

        [Test]
        public void VerifyLog1SucceedsWithMoreThanZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");

            Assert.That(mockLogger.VerifyLog, Throws.Nothing);
        }

        [Test]
        public void VerifyLog2FailsWithZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            Assert.That(() => mockLogger.VerifyLog("Oh no!"),
                Throws.InstanceOf<MockException>().With.Message.StringStarting("Oh no!"));
        }

        [Test]
        public void VerifyLog2SucceedsWithMoreThanZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");

            Assert.That(() => mockLogger.VerifyLog("Oh no!"), Throws.Nothing);
        }

        [Test]
        public void VerifyLog3FailsWhenInvocationCountDoesNotMatchTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("baz");

            Assert.That(() => mockLogger.VerifyLog(Times.Exactly(2)),
                Throws.InstanceOf<MockException>());
        }

        [Test]
        public void VerifyLog3SucceedsWhenInvocationCountMatchesTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("baz");

            Assert.That(() => mockLogger.VerifyLog(Times.Exactly(3)), Throws.Nothing);
        }

        [Test]
        public void VerifyLog4FailsWhenInvocationCountDoesNotMatchTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("baz");

            Assert.That(() => mockLogger.VerifyLog(Times.Exactly(2), "Oh no!"),
                Throws.InstanceOf<MockException>().With.Message.StringStarting("Oh no!"));
        }

        [Test]
        public void VerifyLog4SucceedsWhenInvocationCountMatchesTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("baz");

            Assert.That(() => mockLogger.VerifyLog(Times.Exactly(3), "Oh no!"),
                Throws.Nothing);
        }

        [Test]
        public void VerifyLog5FailsWithZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");

            Assert.That(() =>
                mockLogger.VerifyLog(logEntry => logEntry.Message.StartsWith("baz")),
                Throws.InstanceOf<MockException>());
        }

        [Test]
        public void VerifyLog5SucceedsWithMoreThanZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("foobar");

            Assert.That(() =>
                mockLogger.VerifyLog(logEntry => logEntry.Message.StartsWith("foo")),
                Throws.Nothing);
        }

        [Test]
        public void VerifyLog6FailsWithZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");


            Assert.That(() => mockLogger.VerifyLog(
                    logEntry => logEntry.Message.StartsWith("baz"),
                    "Oh no!"),
                Throws.InstanceOf<MockException>().With.Message.StringStarting("Oh no!"));
        }

        [Test]
        public void VerifyLog6SucceedsWithMoreThanZeroLoggingCalls()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("foobar");

            Assert.That(() =>
                mockLogger.VerifyLog(
                    logEntry => logEntry.Message.StartsWith("foo"),
                    "Oh no!"),
                Throws.Nothing);
        }

        [Test]
        public void VerifyLog7FailsWhenInvocationCountDoesNotMatchTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("foobar");

            Assert.That(() => mockLogger.VerifyLog(
                    logEntry => logEntry.Message.StartsWith("foo"),
                    Times.Exactly(3)),
                Throws.InstanceOf<MockException>());
        }

        [Test]
        public void VerifyLog7SucceedsWhenInvocationCountMatchesTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("foobar");

            Assert.That(() => mockLogger.VerifyLog(
                    logEntry => logEntry.Message.StartsWith("foo"),
                    Times.Exactly(2)),
                Throws.Nothing);
        }

        [Test]
        public void VerifyLog8FailsWhenInvocationCountDoesNotMatchTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("foobar");

            Assert.That(() => mockLogger.VerifyLog(
                    logEntry => logEntry.Message.StartsWith("foo"),
                    Times.Exactly(3),
                    "Oh no!"),
                Throws.InstanceOf<MockException>().With.Message.StringStarting("Oh no!"));
        }

        [Test]
        public void VerifyLog8SucceedsWhenInvocationCountMatchesTimesParameter()
        {
            var mockLogger = new MockLogger();

            mockLogger.Object.Debug("foo");
            mockLogger.Object.Debug("bar");
            mockLogger.Object.Debug("foobar");

            Assert.That(() => mockLogger.VerifyLog(
                    logEntry => logEntry.Message.StartsWith("foo"),
                    Times.Exactly(2),
                    "Oh no!"),
                Throws.Nothing);
        }
    }
}