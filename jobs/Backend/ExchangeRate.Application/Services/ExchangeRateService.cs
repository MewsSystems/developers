using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Services.Interfaces;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ICzechNationalBankService _cnbService;
        private readonly IParserService _parserService;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(ICzechNationalBankService cnbService, 
            IParserService parserService,
            ILogger<ExchangeRateService> logger)
        {
            _cnbService = cnbService;
            _parserService = parserService;
            _logger = logger;
        }

        public async Task<ExchangeRatesBankDTO> GetDailyExchangeRates()
        {
            return await GetExchangeRatesAsync( () => _cnbService.GetDailyExchangeRates(), _parserService.ExchangeRateParseXml);
        }

        public async Task<ExchangeRatesBankDTO> GetExchangeRatesByDate(DateTime date)
        {
            return await GetExchangeRatesAsync(() => _cnbService.GetExchangeRatesByDay(date), _parserService.ExchangeRateParseText);
        }
        public CurrenciesBankDTO GetCurrenciesBank(ExchangeRatesBankDTO rates)
        {
            var currencies = _parserService.CurrencyParse(rates.ExchangeRates);
            return new CurrenciesBankDTO(currencies);
        }

        private async Task<ExchangeRatesBankDTO> GetExchangeRatesAsync(Func<Task<string?>> getExchangeRates, Func<string, List<ExchangeRateBankDTO>> parseRates)
        {
            try
            {
                string? data = await getExchangeRates();
                if(string.IsNullOrWhiteSpace(data))
                {
                    throw new InvalidOperationException("Exchange rate data is empty or null");
                }

                var rateList = parseRates(data);
                return new ExchangeRatesBankDTO(rateList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing exchange rates.");
                throw;
            }
        }

        
    }
}
