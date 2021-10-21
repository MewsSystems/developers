using System;

namespace ExchangeRateUpdater.MessageWriter
{
    public class ConsoleWriter : IWriter
    {
        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
