using ExchangeRateUpdater.Models.Requests;
using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Responses;
using System.Diagnostics;
using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models.Models;

namespace ExchangeRateUpdater.Business.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private IExchangeRateRepository _exchangeRatesRepository;

        public ExchangeRateService(IExchangeRateRepository exchangeRatesRepository)
        {
            _exchangeRatesRepository = exchangeRatesRepository;
        }
        public async Task<List<ExchangeRateResultDto>> GetExchangeRates(IEnumerable<ExchangeRateRequest> currencies, DateTime date, CancellationToken cancellationToken)
        {
            var exchangeRatesByCurrency = new List<ExchangeRate>();

            //Get all the rates for a specified date
            var exchangeRates = await _exchangeRatesRepository.GetExchangeRatesByDateAsync(date, cancellationToken);

            //Get only the requested ones
            var requestedExchangeRates = exchangeRates.Where(p => currencies
                                .Any(e => e.ToString() == p.ToString()))
                                .ToList();

            //Map to a new object that simplifies reading the response
            return requestedExchangeRates.Select(rate => MapToResult(rate)).ToList();
        }

        private ExchangeRateResultDto MapToResult(ExchangeRate rate)
        {
            return new ExchangeRateResultDto(
               new Currency(rate.SourceCurrency.Code),
               new Currency(rate.TargetCurrency.Code),
               rate.Value,
               rate.Date);
        }
    }


}
