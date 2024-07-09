using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Lib.Shared
{
    public class ConsoleLoggerSettings
    {
        public ConsoleLoggerSettings()
        {

        }

        public bool Enabled { get; internal set; }
    }

    public class ConsoleLogger : ILogger
    {
        public ConsoleLoggerSettings Settings { get; }

        public ConsoleLogger(
            ConsoleLoggerSettings logger
            )
        {
            Settings = logger;
        }

        public void Info(string message)
        {
            if (Settings.Enabled)
            {
                Console.WriteLine($"INFO: {message}");
            }
        }

        public void Warn(string message)
        {
            if (Settings.Enabled)
            {
                Console.WriteLine($"WARN: {message}");
            }
        }

        public void Error(string message)
        {
            if (Settings.Enabled)
            {
                Console.WriteLine($"ERROR: {message}");
            }
        }
    }

}
