using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Helpers;
using System.Collections.Generic;
using System.Reflection;

namespace ExchangeRateUpdater.Infrastructure
{
    /// <summary>
    /// In a real world scenario available currencies should be stored in a DB
    /// so that there is no need to commit any code change to modify them.
    /// Also, they will be accessed through EF so that CreateCurrency method won't be required
    /// </summary>
    public class InMemoryReadOnlyCurrenciesRepository : IReadOnlyRepository<Currency>
    {
        public IEnumerable<Currency> GetAll()
        {
            yield return CreateCurrency("AUD");
            yield return CreateCurrency("BRL");
            yield return CreateCurrency("BGN");
            yield return CreateCurrency("CAD");
            yield return CreateCurrency("CNY");
            yield return CreateCurrency("DKK");
            yield return CreateCurrency("EUR");
            yield return CreateCurrency("HKD");
            yield return CreateCurrency("HUF");
            yield return CreateCurrency("ISK");
            yield return CreateCurrency("XDR");
            yield return CreateCurrency("INR");
            yield return CreateCurrency("IDR");
            yield return CreateCurrency("ILS");
            yield return CreateCurrency("JPY");
            yield return CreateCurrency("MYR");
            yield return CreateCurrency("MXN");
            yield return CreateCurrency("NZD");
            yield return CreateCurrency("NOK");
            yield return CreateCurrency("PHP");
            yield return CreateCurrency("PLN");
            yield return CreateCurrency("RON");
            yield return CreateCurrency("SGD");
            yield return CreateCurrency("ZAR");
            yield return CreateCurrency("KRW");
            yield return CreateCurrency("SEK");
            yield return CreateCurrency("CHF");
            yield return CreateCurrency("THB");
            yield return CreateCurrency("TRY");
            yield return CreateCurrency("GBP");
            yield return CreateCurrency("USD");
        }

        private Currency CreateCurrency(string code)
        {
            var ci = typeof(Currency).GetConstructor(
                bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null, 
                types: new[] { typeof(string) }, 
                modifiers: null);

            return (Currency)ci.Invoke(new object[] { code });
        }
    }
}
