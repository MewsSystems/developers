namespace ExchangeRateUpdater.Code.Observability
{
    using System;
    using Serilog;
    using Serilog.Events;

    #region ILogger
    /// <summary>
    /// The ILogger Interface.
    /// </summary>
    /// <remarks>Public interfaces are documented. This could be used to document the code.</remarks>
    public interface ILogger
    {
        /// <summary>
        /// Log an Error message
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">The error exception object</param>
        void LogError(string message, Exception exception);

        /// <summary>
        /// Log a Fatal message
        /// </summary>
        /// <param name="message">The fatal error message to log</param>
        void LogFatal(string message);

        /// <summary>
        /// Log an Information message
        /// </summary>
        /// <param name="message">The information message to log</param>
        void LogInformation(string message);

        /// <summary>
        /// Log a Warning message
        /// </summary>
        /// <param name="message">The warning message to log</param>
        void LogWarning(string message);
    }

    #endregion ILogger

    /// <summary>
    /// Console Logger that will log to stdout
    /// </summary>
    /// <remarks>In a real world application we would have different sinks for logs</remarks>
    public class ConsoleLoggier : ILogger
    {
        public ConsoleLoggier()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        }

        public void LogInformation(string message) => Log.Logger.Information(message);
        public void LogWarning(string message) => Log.Logger.Warning(message);
        public void LogError(string message, Exception exception) => Log.Logger.Error(message, exception);
        public void LogFatal(string message) => Log.Logger.Fatal(message);
    }
}
