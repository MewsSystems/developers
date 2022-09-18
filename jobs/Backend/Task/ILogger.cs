using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	internal interface ILogger
	{
		Task LogInfoAsync(string message);
		Task LogErrorAsync(string message);
	}
}
