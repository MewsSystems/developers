using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Helpers.Interfaces
{
    public interface IDataModifyingService
    {
        Course DeserializeString(string content);
        IEnumerable<CurrencyValue> CommonCurrencies(IEnumerable<Entity> sourceCurrency, IEnumerable<Currency> availableCurrencies);
    }
}