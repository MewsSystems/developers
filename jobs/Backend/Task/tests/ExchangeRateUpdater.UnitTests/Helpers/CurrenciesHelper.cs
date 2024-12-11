using ExchangeRateUpdater.Domain;
using System.Reflection;

namespace ExchangeRateUpdater.UnitTests.Helpers
{
    public class CurrenciesHelper
    {
        public static Currency CZK => CreateCurrency("CZK");
        public static Currency EUR => CreateCurrency("EUR");
        public static Currency USD => CreateCurrency("USD");

        private static Currency CreateCurrency(string code)
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
