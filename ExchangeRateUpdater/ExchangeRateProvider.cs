using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
			int inputLen = currencies.Count();
			if (inputLen == 0)
				return Enumerable.Empty<ExchangeRate>();
			List<ExchangeRate> rates = new List<ExchangeRate>();
			List<Currency> currenciesList = new List<Currency>(currencies);
			BankConnection.BankConnector privateBank = new BankConnection.BankConnector();
			if (privateBank.Connect())
			{
				try
				{
					for (int i = 0; i < inputLen; i++)
					{
						Currency cur1 = currenciesList[i];
						for (int j = 0; j < inputLen; j++)
						{
							Currency cur2 = currenciesList[j];
							if (cur1 == cur2)
								continue;
							string currency1 = cur1.Code;
							string currency2 = cur2.Code;

							var exchangeValue = privateBank.GetPairValue(currency1, currency2);
							decimal decimalValue = -1;
							if (decimal.TryParse(exchangeValue, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out decimalValue))
							{
								ExchangeRate rate = new ExchangeRate(cur1, cur2, decimalValue);
								rates.Add(rate);
							}
						}
					}
					return rates.AsEnumerable<ExchangeRate>();
				}
				catch (System.Exception ex)
				{
					System.Console.WriteLine("Error: " + ex.Message);
				}
				finally
				{ privateBank.Disconnect(); }
			}

			return Enumerable.Empty<ExchangeRate>();
        }
    }
}
