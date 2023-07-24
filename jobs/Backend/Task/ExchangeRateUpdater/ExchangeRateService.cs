using System.Collections.Generic;

namespace ExchangeRateUpdater
{
   public interface IExchangeRateService
   {
      IEnumerable<ExchangeRate> Get();
   }

   public class ExchangeRateService : IExchangeRateService
   {
      public IEnumerable<ExchangeRate> Get()
      {
         return new ExchangeRate[]
         {
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24)
         };
      }
   }
}