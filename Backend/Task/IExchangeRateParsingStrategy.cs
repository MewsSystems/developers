using System;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateParsingStrategy
    {
        ExchangeRate Parse(string line);
        bool TryParse(string line, out ExchangeRate exchangeRate);
    }
}
