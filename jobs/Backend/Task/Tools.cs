using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	class Tools
	{
		public static void WriteLine(string message, ConsoleColor colour)
		{
			Console.ForegroundColor = colour;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}
