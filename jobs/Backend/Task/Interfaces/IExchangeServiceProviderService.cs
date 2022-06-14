using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using models.ExchangeRateUpdater;

public interface IExchangeServiceProviderService
{
    List<ExchangeRate> GetExchangeRates(List<CurrencyModel> currencies, Settings settings);
}

