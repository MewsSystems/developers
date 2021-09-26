using System;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace ExchangeRateUpdater.Utilities.Logging
{
    public sealed class AppLogger : IAppLogger
    {
        private const string loggerName = "ExchangeRateUpdater";
        
        private readonly ILog _logger;
        
        public AppLogger()
        {
            _logger = LogManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);

            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Configured = true;

            var logger = (Logger)hierarchy.GetLogger(loggerName);
            logger.Level = log4net.Core.Level.All;
            
            XmlConfigurator.Configure();

            // TODO: Simplified configuration, adding only one appender with basic configuration
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}
