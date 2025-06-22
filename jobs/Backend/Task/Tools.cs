using System;

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
