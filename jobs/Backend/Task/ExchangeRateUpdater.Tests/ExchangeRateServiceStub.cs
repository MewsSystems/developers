using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateServiceStub : IExchangeRateService
{
   private List<ExchangeRateResponse> _rates = new List<ExchangeRateResponse>();

   public void Add(string currencyCode, decimal rate)
   {
      _rates.Add(new ExchangeRateResponse
      {
         CurrencyCode = currencyCode,
         Rate = rate
      });
   }
   public Task<ExchangeRateServiceResponse> Get()
   {
      return Task.FromResult(new ExchangeRateServiceResponse()
      {
         Rates = _rates.ToArray()
      });
   }
}