using Infrastructure.Extensions;

namespace Infrastructure.Clients.Cnb
{
    public class CnbzClient : ICnbzClient
    {

        private readonly HttpClient _httpClient;

        public CnbzClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetExchangeRateAmountCsvAsync(DateTimeOffset date)
        {
            return _httpClient.GetStringAsync($"/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={date.ToPragueDateTime():dd.MM.yyyy}");
        }
    }
}
