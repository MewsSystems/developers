using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	internal interface IWebRequestService
	{
		Task<string> GetAsStringAsync(Uri uriToGet);
	}
}
