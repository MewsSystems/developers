using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
	internal class Logger : ILogger
	{
		public async Task LogInfoAsync(string message)
		{
			await Console.Out.WriteLineAsync(message).ConfigureAwait(false);
		}

		public async Task LogErrorAsync(string message)
		{
			await Console.Error.WriteLineAsync(message).ConfigureAwait(false);
		}
	}
}
