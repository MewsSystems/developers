
using ExchangeRate.Infrastructure.ExternalServices.Builders;
using ExchangeRate.Infrastructure.ExternalServices.Configs;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank.Models;

namespace ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly CzechNationalBanckConfigs _configs;
        private readonly IBuildExchangeRates _builder;

        public ExchangeRatesService(IInfraConfigs config, IBuildExchangeRates builder)
        {
            _configs = config.CzechNatBank;
            _builder = builder;
        }

        public async Task<List<ExchangeRateBank>> GetExchangeRatesByDay(DateTime date)
        {
            using HttpClient client = new HttpClient();
            var dateFormated = date.ToString("dd.MM.yyyy");
            var urlBank = $"{_configs.Url}date={dateFormated}";
            string data = await client.GetStringAsync(urlBank);

            return _builder.BuildExchangeRates(data);

        }

        public string Rates(int rates)
        {
            return $"Successfully retrieved {rates} exchange rates:";
        }

    }
}
