using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;

namespace CzechNationalBankApi
{
    /// <summary>
    /// Provides various data from the Czech bank api.
    /// https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/index.html?date=
    /// </summary>
    public class CzechBankApiService : ICzechBankApiService
    {
        private readonly ILogger<CzechBankApiService> _logger;
        private readonly HttpClient _httpClient;

        public CzechBankApiService(ILogger<CzechBankApiService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CzechExchangeItemDto>> GetExchangeRatesAsync()
        {
            var czechItems = new List<CzechExchangeItemDto>();

            //Two stage check - encapsulate within one method
            //  Some currencies are main currencies, then there are "other" ones. Just check both inside the method and do out best to return something :-)
            //      https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=11.07.2024
            //  Second check, and final: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=2024&month=6
            //All values from the two api endpoints come back like: Country|Currency|Amount|Code|Rate

            var date = DateTime.UtcNow;

            var dailyResponse = await _httpClient.GetAsync($"en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={date:dd.MM.yyyyy}");

            dailyResponse.EnsureSuccessStatusCode();

            var dailyResponseStream = await dailyResponse.Content.ReadAsStreamAsync();

            czechItems.AddRange(ParseStreamAsCSV(dailyResponseStream));


            var fxResponse = await _httpClient.GetAsync($"en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year={date:yyyy}&month={date:MM}");

            fxResponse.EnsureSuccessStatusCode();

            var fxResponseStream = await fxResponse.Content.ReadAsStreamAsync();

            czechItems.AddRange(ParseStreamAsCSV(fxResponseStream));

            //Could have done this using hashsets instead
            var duplicates = czechItems.GroupBy(x => x.Code.ToLower()).Where(g => g.Count() > 1);

            if (duplicates.Any())
            {
                throw new Exception("Duplicate currency code information found");
            }

            return czechItems;
        }

        private IEnumerable<CzechExchangeItemDto> ParseStreamAsCSV(Stream data)
        {
            var config = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                Delimiter = "|",
                ShouldSkipRecord = r => r.Row[0].Contains("#") //This removes the weird extra row in the response from the API
            };

            using var reader = new StreamReader(data);

            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<CzechExchangeItemDto>().ToList();
        }
    }
}
