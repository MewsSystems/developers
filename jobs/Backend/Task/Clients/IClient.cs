using System.Threading.Tasks;
using System;
using ExchangeRates.Contracts;

namespace ExchangeRates.Clients
{
	public interface IClient
	{
		Task<string> GetExchangeRatesAsync(DateOnly? date);
	}
}
