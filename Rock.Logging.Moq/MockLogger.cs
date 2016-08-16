using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

// ReSharper disable ExplicitCallerInfoArgument
namespace Rock.Logging.Moq
{
    public class MockLogger : Mock<ILogger>
    {
        public MockLogger()
            : this(LogLevel.Debug)
        {
        }

        public MockLogger(LogLevel loggingLevel)
            : base(MockBehavior.Strict)
        {
            Setup(loggingLevel);
        }

        private void Setup(LogLevel loggingLevel)
        {
            Setup(x => x.IsEnabled(It.Is<LogLevel>(level => level >= loggingLevel))).Returns(true);
            Setup(x => x.IsEnabled(It.Is<LogLevel>(level => level < loggingLevel))).Returns(false);
            Setup(x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(0));
        }

        public void VerifyLog()
        {
            Verify(x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        public void VerifyLog(string failMessage)
        {
            Verify(
                x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                failMessage);
        }

        public void VerifyLog(Times times)
        {
            Verify(
                x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times);
        }

        public void VerifyLog(Times times, string failMessage)
        {
            Verify(
                x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times,
                failMessage);
        }

        public void VerifyLog(Expression<Func<ILogEntry, bool>> match)
        {
            Verify(x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        public void VerifyLog(Expression<Func<ILogEntry, bool>> match, string failMessage)
        {
            Verify(
                x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                failMessage);
        }

        public void VerifyLog(Expression<Func<ILogEntry, bool>> match, Times times)
        {
            Verify(
                x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times);
        }

        public void VerifyLog(Expression<Func<ILogEntry, bool>> match, Times times, string failMessage)
        {
            Verify(
                x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times,
                failMessage);
        }
    }
}
// ReSharper restore ExplicitCallerInfoArgument
