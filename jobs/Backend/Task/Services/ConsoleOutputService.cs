using ExchangeRateUpdater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ConsoleOutputService : IOutputService
    {
        public void WriteMessage(string text)
        {
            Console.WriteLine(text);
        }
    }
}
