using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

// ReSharper disable ExplicitCallerInfoArgument
namespace Rock.Logging.Moq
{
    /// <summary> 
    /// Defines a mock implementation of the <see cref="ILogger"/> interface.
    /// </summary>
    /// <remarks>
    /// This class is designed to be easier to set up and verify than manually instantiating
    /// a <see cref="Mock{T}"/> object of type <see cref="ILogger"/>.
    /// </remarks>
    public class MockLogger : Mock<ILogger>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="MockLogger"/> class with a logging
        /// level of <see cref="LogLevel.Debug"/>.
        /// </summary>
        public MockLogger()
            : this(LogLevel.Debug)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="MockLogger"/> class with the specified
        /// logging level.
        /// </summary>
        /// <param name="loggingLevel">The logging level of the mock logger.</param>
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

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger.
        /// </summary>
        public void VerifyLog()
        {
            Verify(x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger, specifying
        /// a failure error message.
        /// </summary>
        /// <param name="failMessage">The message to show if verification fails.</param>
        public void VerifyLog(string failMessage)
        {
            Verify(
                x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                failMessage);
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger the specified
        /// number of times.
        /// </summary>
        /// <param name="times">
        /// The number of times that a logging operation should be performed.
        /// </param>
        public void VerifyLog(Times times)
        {
            Verify(
                x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times);
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger the specified
        /// number of times, specifying a failure error message.
        /// </summary>
        /// <param name="times">
        /// The number of times that a logging operation should be performed.
        /// </param>
        /// <param name="failMessage">The message to show if verification fails.</param>
        public void VerifyLog(Times times, string failMessage)
        {
            Verify(
                x => x.LogAsync(It.IsAny<ILogEntry>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times,
                failMessage);
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger that matches
        /// a predicate.
        /// </summary>
        /// <param name="match">
        /// The predicate that determines whether a log entry satifies this verification.
        /// </param>
        public void VerifyLog(Expression<Func<ILogEntry, bool>> match)
        {
            Verify(x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger that matches
        /// a predicate.
        /// </summary>
        /// <param name="match">
        /// The predicate that determines whether a log entry satifies this verification.
        /// </param>
        /// <param name="failMessage">The message to show if verification fails.</param>
        public void VerifyLog(Expression<Func<ILogEntry, bool>> match, string failMessage)
        {
            Verify(
                x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                failMessage);
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger that matches
        /// a predicate.
        /// </summary>
        /// <param name="match">
        /// The predicate that determines whether a log entry satifies this verification.
        /// </param>
        /// <param name="times"></param>
        public void VerifyLog(Expression<Func<ILogEntry, bool>> match, Times times)
        {
            Verify(
                x => x.LogAsync(It.Is(match), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                times);
        }

        /// <summary>
        /// Verifies that a logging operation was performed on the mock logger that matches
        /// a predicate.
        /// </summary>
        /// <param name="match">
        /// The predicate that determines whether a log entry satifies this verification.
        /// </param>
        /// <param name="times">
        /// The number of times that a logging operation should be performed.
        /// </param>
        /// <param name="failMessage">The message to show if verification fails.</param>
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
