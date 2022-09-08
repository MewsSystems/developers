using Application.Services.Interfaces;
using Domain.Core;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private static Dictionary<string, ExchangeRate> _exchangeRatesCache = new Dictionary<string, ExchangeRate>();
        private readonly ICNBGateway _gateway;

        public ExchangeRateProvider(ICNBGateway gateway)
        {
            _gateway = gateway;
        }

        public void LoadData()
        {
            _exchangeRatesCache.Clear();
            var data = _gateway.LoadDataFromServer();
            ParseData(data);
        } 


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                if (_exchangeRatesCache.ContainsKey(currency.Code))
                {
                    exchangeRates.Add(_exchangeRatesCache[currency.Code]);
                }
            }

            return exchangeRates;
        }

        private static void ParseData(string rawData)
        {
            try
            {
                var data = rawData.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in data)
                {
                    string header = "země|měna|množství|kód|kurz";
                    if (line.Equals(header))
                        continue;

                    var regex = new System.Text.RegularExpressions.Regex(@"^([0-9]{2})[.]([0-9]{2})[.]([0-9]{4})");
                    if (regex.IsMatch(line))
                        continue;

                    ParseExchangeRate(line);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ParseExchangeRate(string rawData)
        {
            try
            {
                char separator = '|';

                string[] parsedDataArray = rawData.Split(separator);
                if (parsedDataArray.Length < 0)
                    return;

                var cultureInfo = new CultureInfo("de-DE");

                decimal.TryParse(parsedDataArray[4],NumberStyles.Currency,cultureInfo, out var exchangeRate);
                //will use only currency code
                _exchangeRatesCache.Add(
                    parsedDataArray[3],
                    new ExchangeRate(new Currency(parsedDataArray[3]), new Currency("CZK"), exchangeRate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ExchangeRate Convert(Currency sourceCurrency, Currency targetCurrency)
        {
            if (sourceCurrency.Code.ToLower() == "czk")
            {
                return new ExchangeRate(sourceCurrency,targetCurrency, FromCzk(targetCurrency.Code));
            }

            if (sourceCurrency.Code.ToLower() == "czk")
            {
                return new ExchangeRate(sourceCurrency, targetCurrency, ToCzk(sourceCurrency.Code));
            }

            decimal czkValue = ToCzk(sourceCurrency.Code);
            var total = czkValue / ToCzk(targetCurrency.Code);
            return new ExchangeRate(sourceCurrency, targetCurrency, total);
        }

        public decimal FromCzk(string currencyCode)
        {
            return 1 / _exchangeRatesCache[currencyCode].Value;
        }

        public decimal ToCzk(string currencyCode)
        {
            return _exchangeRatesCache[currencyCode].Value;
        }


        public IEnumerable<Currency> currencies = new[]
         {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
    }
}
