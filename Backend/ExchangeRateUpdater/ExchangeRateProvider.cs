using ExchangeRateUpdater.Interfaces;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        public const string CZKcode = "CZK";
        private const string CnbUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private Currency CZK => new Currency(CZKcode);

        private readonly IConnector _connector;
        private readonly IParser _parser;

        public ExchangeRateProvider(IConnector connector, IParser parser)
        {
            this._connector = connector;
            this._parser = parser;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string source;

            try
            {
                source = this._connector.DownloadTxtFile(GetUrl());
            }
            catch (Exception ex)
            {
                throw new Exception("Obtaining data from CNB failed. See inner exception for more info", ex);
            }
            Dictionary<string, decimal> map = this._parser.Parse(source);
            foreach (Currency currency in currencies)
            {
                string code = currency.Code?.ToUpperInvariant()?.Trim();
                if (!string.IsNullOrEmpty(code) && map.ContainsKey(code))
                {
                    yield return new ExchangeRate(currency, this.CZK, map[code]);
                }
            }
        }



        private static string GetUrl(DateTime? date = null)
        {
            if (date.HasValue)
            {
                return $"{CnbUrl}?date={date.Value:dd.MM.yyyy}";
            }
            return CnbUrl;
        }

    }
}
