using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.App;

public interface IExchangeRateClient
{
    Task<string> GetExchangeRateAsync(DateTime date);
}