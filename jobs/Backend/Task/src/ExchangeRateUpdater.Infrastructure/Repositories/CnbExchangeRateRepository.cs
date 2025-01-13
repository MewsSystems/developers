using ExchangeRateUpdater.Application.Contracts.Persistence;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Services.Providers;

namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    public class CnbExchangeRateRepository : ICnbExchangeRateRepository
    {
        private readonly ICnbExchangeService _cnbExchangeService;

        public CnbExchangeRateRepository(ICnbExchangeService cnbExchangeService)
        {
            _cnbExchangeService = cnbExchangeService;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? date, CancellationToken cancellationToken)
        {
            var cnbExRates = await _cnbExchangeService.GetExchangeRatesByDateAsync(date, cancellationToken);
            return cnbExRates.Select(x => 
                new ExchangeRate(new Currency(x.CurrencyCode), new Currency("CZK"), x.Rate / x.Amount));
        }
    }
}
