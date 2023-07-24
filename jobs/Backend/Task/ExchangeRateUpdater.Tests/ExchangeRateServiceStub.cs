using System.Collections.Generic;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateServiceStub : IExchangeRateService
{
   private List<ExchangeRate> _rates = new List<ExchangeRate>();

   public void Add(string sourceCurrencyCode, string targetCurrencyCode, decimal value)
   {
      _rates.Add(new ExchangeRate(
         new Currency(sourceCurrencyCode), new Currency(targetCurrencyCode), value));
   }

   public IEnumerable<ExchangeRate> Get()
   {
      return _rates;
   }
}