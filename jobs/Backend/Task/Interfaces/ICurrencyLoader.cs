using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    public interface ICurrencyLoader
    {
        IEnumerable<Currency> LoadCurrencies();
    }
}
