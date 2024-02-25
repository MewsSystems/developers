using System;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;

namespace Logger
{
    public class LoggerService
    {
        public ILogger logger = null;

        public LoggerService()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
                                            builder.AddConsole()
                                            .AddDebug()
                                            .SetMinimumLevel(LogLevel.Debug));

            logger = loggerFactory.CreateLogger<Program>();
        }
    }
}
