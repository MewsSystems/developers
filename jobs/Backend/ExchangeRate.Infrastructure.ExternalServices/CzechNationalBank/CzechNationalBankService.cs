using ExchangeRate.Infrastructure.ExternalServices.Configs;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank
{
    public class CzechNationalBankService : ICzechNationalBankService
    {
        private readonly CzechNationalBanckConfigs _configs;
        private readonly ILogger<CzechNationalBankService> _logger;
        public CzechNationalBankService(IInfraConfigs config, ILogger<CzechNationalBankService> logger)
        {
            _configs = config.CzechNatBank;
            _logger = logger;
        }

        public async Task<string> GetExchangeRatesByDay(DateTime date)
        {
            try
            {
                using HttpClient client = new HttpClient();
                var dateFormated = date.ToString("dd.MM.yyyy");
                var urlBank = $"{_configs.BaseUrl}/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={dateFormated}";
                string data = await client.GetStringAsync(urlBank);

                return data;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, $"HTTP request error while fetching exchange rates for date: {date.ToString("dd.MM.yyyy")}.");
                throw new Exception("An error occurred while fetching exchange rates. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while fetching exchange rates for date: {date.ToString("dd.MM.yyyy")}.");
                throw new Exception("An unexpected error occurred while fetching exchange rates. Please try again later.");
            }


        }

        public async Task<string> GetDailyExchangeRates()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var urlBank = $"{_configs.BaseUrl}/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
                string data = await client.GetStringAsync(urlBank);

                return data;
            }
            catch (HttpRequestException httpEx)
            {                
                _logger.LogError(httpEx, "HTTP request error while fetching daily exchange rates.");               
                throw new Exception("An error occurred while fetching daily exchange rates. Please try again later.");
            }
            catch (Exception ex)
            {              
                _logger.LogError(ex, "Unexpected error while fetching daily exchange rates.");                
                throw new Exception("An unexpected error occurred while fetching daily exchange rates. Please try again later.");
            }

        }

        public string Rates(int rates)
        {
            return $"Successfully retrieved {rates} exchange rates:";
        }

    }
}
