using System.Text;
using ExchangeRate.CrossCutting.Configurations;
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

        public async Task<string?> GetExchangeRatesByDay(DateTime date)
        {
            var dateFormatted = date.ToString("dd.MM.yyyy");
            try
            {
                using HttpClient client = new HttpClient();
                var urlBank = new StringBuilder();
                urlBank.Append(_configs.BaseUrl);
                urlBank.Append("/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=");
                urlBank.Append(dateFormatted);
                string? data = await client.GetStringAsync(urlBank.ToString());

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching exchange rates for date: {dateFormatted}");
                throw;
            }
        }

        public async Task<string?> GetDailyExchangeRates()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var urlBank = new StringBuilder();
                urlBank.Append(_configs.BaseUrl);
                urlBank.Append("/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");
                string? data = await client.GetStringAsync(urlBank.ToString());
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching exchange rates for today");
                throw;
            }
        }

    }
}
