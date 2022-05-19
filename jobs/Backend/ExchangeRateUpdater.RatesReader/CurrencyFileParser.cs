using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace ExchangeRateUpdater.RatesReader
{
    internal static class CurrencyFileParser
    {
        internal static IEnumerable<ExchangeRateReadModel> ParseFileToExchangeRatesReadModel(string response)
        {
            IEnumerable<ExchangeRateReadModel> exchangeRates;
            response = RemoveFirstRow(response);
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = "|", Encoding = Encoding.UTF8 };
            using (var reader = new StringReader(response))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<ExchangeRateReadModelMap>();
                exchangeRates = csv.GetRecords<ExchangeRateReadModel>().ToList();
            }
            return exchangeRates;
        }

        private static string RemoveFirstRow(string response)
            => response.Substring(response.IndexOf('\n') + 1);
        

        public sealed class ExchangeRateReadModelMap : ClassMap<ExchangeRateReadModel>
        {
            public ExchangeRateReadModelMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.CurrencyCode).Name("Code");
                Map(m => m.Rate).Name("Rate");
                Map(m => m.Amount).Name("Amount");
            }
        }
    }
}