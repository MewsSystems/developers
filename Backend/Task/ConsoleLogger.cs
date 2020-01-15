using System;

namespace ExchangeRateUpdater
{
    internal class ConsoleLogger : ILogger
    {
        public static readonly ConsoleLogger Instance = new ConsoleLogger();

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
