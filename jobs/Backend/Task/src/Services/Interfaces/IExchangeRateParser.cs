using System.Collections.Generic;

namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IExchangeRateParser
    {
        IEnumerable<(string Code, decimal Amount, decimal Rate)> ParseRates(string data);
    }
} 