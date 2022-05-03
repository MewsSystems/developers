using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ExchangeRateUpdater.Integration
{
    public class CnbApiClient : ICnbApiClient
    {
        private readonly HttpClient _httpClient;

        public CnbApiClient(HttpClient httpClient)
        {
            //TODO: Inject ILogger here
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CnbExchangeRate>> GetBasicRatesAsync()
        {
            var czCulture = CultureInfo.GetCultureInfo("cs-CZ");
            var url = $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={DateTime.Today.ToString("dd.MM.yyyy", czCulture)}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new CnbIntegrationException(response.ReasonPhrase, response.StatusCode);
            }

            using (var responseBodyStream = await response.Content.ReadAsStreamAsync())
            {
                return await ReadExchangeRatesFromResponseAsync(czCulture, responseBodyStream);
            }
        }

        public async Task<IEnumerable<CnbExchangeRate>> GetOtherCurrenciesRatesAsync()
        {
            var date = DateTime.Today;
            var url = $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.txt?rok={date.Year}&mesic={date.Month}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new CnbIntegrationException(response.ReasonPhrase, response.StatusCode);
            }

            using (var responseBodyStream = await response.Content.ReadAsStreamAsync())
            {
                var czCulture = CultureInfo.GetCultureInfo("cs-CZ");
                return await ReadExchangeRatesFromResponseAsync(czCulture, responseBodyStream);
            }
        }

        private static async Task<IEnumerable<CnbExchangeRate>> ReadExchangeRatesFromResponseAsync(CultureInfo czCulture, Stream responseBodyStream)
        {
            using (StreamReader reader = new StreamReader(responseBodyStream))
            {
                //first line should get ignored
                string? firstLine = await reader.ReadLineAsync();

                var config = new CsvConfiguration(czCulture)
                {
                    IgnoreBlankLines = true,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    Delimiter = "|"
                };

                using (var csvReader = new CsvReader(reader, config))
                {
                    try
                    {
                        var rows = csvReader.GetRecords<CnbExchangeRate>();
                        return rows.ToList();
                    }
                    //Handle correct exception thrown from the csvHelper nuget
                    catch (Exception ex)
                    {
                        //TODO: _logger.LogError
                        return Enumerable.Empty<CnbExchangeRate>();
                    }
                }
            }
        }
    }
}
