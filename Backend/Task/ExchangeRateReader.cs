using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;

namespace ExchangeRateUpdater
{
    public class ExchangeRateReader : IDisposable
    {
        StreamReader streamReader;
        IEnumerable<Currency> currencies;
        Currency baseCode;
        public int Peek() => streamReader.Peek();

        public ExchangeRateReader(string webPage, IEnumerable<Currency> currencyCodes, string baseCode)
        {
            this.currencies = currencyCodes;
            this.baseCode = IsSearchedCode(baseCode) ?? throw new ArgumentException();

            using (HttpClient client = new HttpClient())
            {
                Stream stream = client.GetStreamAsync(webPage).GetAwaiter().GetResult();
                streamReader = new StreamReader(stream);
            }

            ReadHeader();
        }

        public void ReadHeader()
        {
            streamReader.ReadLine();
            streamReader.ReadLine();
        }

        public ExchangeRate Read()
        {
            // format of the line :
            // Country|Currency|Amount|Code|Rate
            string[] tokens = streamReader.ReadLine().Split('|');
            string code = tokens[3];

            Currency currency = IsSearchedCode(code);

            if (currency != null && decimal.TryParse(tokens[4], out decimal exchangeRate))
            {
                int amount = int.Parse(tokens[2]);

                if (amount == 100)
                    exchangeRate = exchangeRate / 100;

                return new ExchangeRate(currency, baseCode, exchangeRate);
            }

            return null;
        }

        private Currency IsSearchedCode(string code)
        {
            foreach (Currency c in currencies)
                if (c.Code == code)
                    return c;

            return null;
        }

        public void Dispose()
        {
            streamReader?.Dispose();
        }

        public void ChangeWebPage(string webPage)
        {
            streamReader.Dispose();

            using (HttpClient client = new HttpClient())
            {
                Stream stream = client.GetStreamAsync(webPage).GetAwaiter().GetResult();
                streamReader = new StreamReader(stream);

                ReadHeader();
            }
        }
    }
}
