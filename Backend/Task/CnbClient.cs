using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbClient : IExchangeRateClient
    {
        // https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/
        private const int HeaderLinesCount = 2;
        public const string GetExchangeRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string TargetCurrencyCode = "CZK";

        private readonly IHttpClient _httpClient;

        public CnbClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            try
            {
                var response = await _httpClient.GetAsync(GetExchangeRatesUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return Parse(content);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get response from CNB", ex);
            }
        }

        private IEnumerable<ExchangeRate> Parse(string source)
        {
            var lines = source.Split(new [] { Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);
            return lines.Skip(HeaderLinesCount).Select(ParseLine).ToArray();
        }

        private ExchangeRate ParseLine(string line)
        {
            try
            {
                var items = line.Split('|');
                var record = new
                {
                    Country = items[0],
                    Name = items[1],
                    Amount = int.Parse(items[2]),
                    Code = items[3],
                    Rate = decimal.Parse(items[4])
                };
                return new ExchangeRate(new Currency(record.Code), new Currency(TargetCurrencyCode), record.Rate/record.Amount);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Failed to parse line: '{line}'", ex);
            }
        }
    }
}