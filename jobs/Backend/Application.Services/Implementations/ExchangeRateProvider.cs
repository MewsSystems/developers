using Application.Services.Interfaces;
using Application.Services.Extensions;
using Domain.Core;
using Domain.Model;
using System.Globalization;


namespace Application.Services.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICNBGateway _gateway;

        public ExchangeRateProvider(ICNBGateway gateway)
        {
            _gateway = gateway;
        }

        public void LoadData()
        {
            ExchangeRateCache.Data.Clear();
            var rawdata = _gateway.LoadDataFromServer();
            var data = rawdata.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in data)
            {
                if (!line.IsToIgnoreLine())
                {
                    var rate = line.ParseExchangeRate();
                    ExchangeRateCache.Data.Add(rate.Key, rate.Value);
                }
            }
        
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
                if (ExchangeRateCache.Data.ContainsKey(currency.Code))
                {
                    exchangeRates.Add(ExchangeRateCache.Data[currency.Code]);
                }
            }

            return exchangeRates;
        }

        public ExchangeRate Convert(Currency sourceCurrency, Currency targetCurrency)
        {
            if (sourceCurrency.Code.ToLower() == "czk")
            {
                return new ExchangeRate(sourceCurrency, targetCurrency, FromCzk(targetCurrency.Code));
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
            return 1 / ExchangeRateCache.Data[currencyCode].Value;
        }

        public decimal ToCzk(string currencyCode)
        {
            return ExchangeRateCache.Data[currencyCode].Value;
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