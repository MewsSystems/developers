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
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = "|", Encoding = Encoding.UTF8 };
            using (var reader = new StringReader(response))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<ExchangeRateReadModelMap>();
                exchangeRates = csv.GetRecords<ExchangeRateReadModel>();
            }
            return exchangeRates;
        }

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