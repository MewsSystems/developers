using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
   public class ExchangeRateProvider
   {
      private readonly IExchangeRateService _exchangeRateService;
      public ExchangeRateProvider(IExchangeRateService exchangeRateService)
      {
         _exchangeRateService = exchangeRateService ??
            throw new ArgumentNullException(nameof(exchangeRateService));
      }

      /// <summary>
      /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
      /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
      /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
      /// some of the currencies, ignore them.
      /// </summary>
      public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
      {
         var currencyCodes = currencies.Select(currency => currency.Code);
         var response = await _exchangeRateService.Get();

         return
            response.Rates
               .Where(rate => currencyCodes.Contains(rate.CurrencyCode))
               .Select(rate =>
                  new ExchangeRate(new Currency(rate.CurrencyCode), new Currency("CZK"), rate.Rate));
      }
   }
}
