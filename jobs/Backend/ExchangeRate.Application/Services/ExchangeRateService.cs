using ExchangeRate.Application.DTOs;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank;

namespace ExchangeRate.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ICzechNationalBankService _cnbService;
        private readonly ExchangeRateParserService _parserService;

        public ExchangeRateService(ICzechNationalBankService cnbService, ExchangeRateParserService parserService)
        {
            _cnbService = cnbService;
            _parserService = parserService;
        }
        public async Task<List<ExchangeRateBankDTO>> GetDailyExchangeRates()
        {

            string? data = await _cnbService.GetDailyExchangeRates();
            var rateList = _parserService.ParseXml(data);
            return rateList;
        }

        public async Task<List<ExchangeRateBankDTO>> GetExchangeRatesByDay(DateTime date)
        {
            string? data = await _cnbService.GetExchangeRatesByDay(date);
            var rateList = _parserService.ParseText(data);
            return rateList;
        }
    }
}
