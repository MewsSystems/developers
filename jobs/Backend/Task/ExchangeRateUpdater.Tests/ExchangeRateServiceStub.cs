using System.Collections.Generic;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateServiceStub : IExchangeRateService
{
   private List<ExchangeRate> _rates = new List<ExchangeRate>();

   public void Add(string currencyCode, decimal value)
   {
      _rates.Add(new ExchangeRate(new Currency(currencyCode), new Currency("CZK"), value));
   }

   public IEnumerable<ExchangeRate> Get()
   {
      return _rates;
   }
}