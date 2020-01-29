using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {

        private const string CzechKoruna = "CZK";

        private const string cnbUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        { 
            var result = new List<ExchangeRate>();
            WebClient webClient = new WebClient();
            Currency targetCurrency;
            if ((targetCurrency = currencies.FirstOrDefault(i => i.Code == CzechKoruna))!=null)
            {
                var request = webClient.OpenRead(cnbUrl);
                if (request != null)
                {
                    using (var reader = new StreamReader(request))
                    {
                        reader.ReadLine();
                        string rate;
                        while ((rate = reader.ReadLine()) != null)
                        {
                            decimal currencyFactor;
                            decimal rateValue;
                            var values = rate.Split('|');
                            if (values.Length == 5)
                            {
                                Currency sourceCurrency;
                                if ((sourceCurrency = currencies.FirstOrDefault(i => i.Code == values[3])) != null)
                                {
                                    if (decimal.TryParse(values[2], out currencyFactor) &&
                                        decimal.TryParse(values[4], out rateValue))
                                        result.Add(new ExchangeRate(sourceCurrency, targetCurrency,
                                            rateValue / currencyFactor));
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
