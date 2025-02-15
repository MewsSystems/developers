using ExchangeRate.Application.DTOs;
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

        public async Task<ExchangeRatesBankDTO> GetExchangeRatesByDay(DateTime date)
        {
            return await GetExchangeRatesAsync(() => _cnbService.GetExchangeRatesByDay(date), _parserService.ExchangeRateParseText);
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
                var currencyList = _parserService.CurrencyParse(rateList);
                return new ExchangeRatesBankDTO(rateList, currencyList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing exchange rates.");
                throw;
            }
        }
        
    }
}
