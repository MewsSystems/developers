using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using models.ExchangeRateUpdater;

public interface IExchangeRateValidationsHelper
{
    bool ValidateCurrency(string currency, Settings settings);
}

