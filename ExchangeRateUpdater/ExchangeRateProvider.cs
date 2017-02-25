using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using System.IO;
using System.Globalization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        const string _query = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

        string LoadFile(Uri uri)
        {
            var req = WebRequest.Create(uri);
            using (var resp = req.GetResponse())
            {
                using (var stream = resp.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        ExchangeRate ParseExchangeRate(string text)
        {
            var line = text.Split('|');
            if (line.Length != 5) 
                throw new ApplicationException("Unexpected length of datasource line");
            var rate = decimal.Parse(line[4], CultureInfo.InvariantCulture);
            var amount = int.Parse(line[2]);
            var code = line[3];
            return new ExchangeRate(new Currency(code), new Currency("CZK"), rate / amount);
        }

        IEnumerable<ExchangeRate> ParseExchangeRates(string page)
        {
            return page.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(2)
                .Select(ParseExchangeRate);
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var page = LoadFile(new Uri(_query));
            return ParseExchangeRates(page)
                .Where(exr => currencies
                       .Contains(exr.SourceCurrency, new CurrencyComparrer()));
        }

        public class CurrencyComparrer : IEqualityComparer<Currency>
        {
            public bool Equals(Currency x, Currency y)
            {
                return x.Code == y.Code;
            }

            public int GetHashCode(Currency obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
