﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ExchangeRateUpdater.Helpers
{
    public class Logger
    {
        public ILogger logger;
        public Logger() { 
            var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                }));

            logger = loggerFactory.CreateLogger<Logger>();
        }
    }
}