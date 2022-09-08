using Application.Services.Interfaces;
using Domain.Core;
using Domain.Model;
using System;
using System.Collections.Generic;
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
            return _exchangeRatesCache.Values.Where(x => x.TargetCurrency == this.currencies);
        }

        private static void ParseData(string rawData)
        {
            try
            {
                string header = "země|měna|množství|kód|kurz";
                if (rawData.Equals(header))
                    return;

                var regex = new System.Text.RegularExpressions.Regex(@"^([0-9]{2})[.]([0-9]{2})[.]([0-9]{4})");
                if (regex.IsMatch(rawData))
                    return;

                ParseExchangeRate(rawData);
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

                Decimal.TryParse(parsedDataArray[4], out var exchangeRate);
                //will use only currency code
                _exchangeRatesCache.Add(
                    parsedDataArray[3],
                    new ExchangeRate(new Currency("CZK"), new Currency(parsedDataArray[3]), exchangeRate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
