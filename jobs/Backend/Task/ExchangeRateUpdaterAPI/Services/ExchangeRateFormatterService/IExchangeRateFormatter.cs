using System;
using ExchangeRateUpdater;

namespace ExchangeRateUpdaterAPI.Services.ExchangeRateFormatterService
{
    public interface IExchangeRateFormatter
    {
        IEnumerable<ExchangeRate> FormatExchangeRates(string exchangeRateData);
    }
}

