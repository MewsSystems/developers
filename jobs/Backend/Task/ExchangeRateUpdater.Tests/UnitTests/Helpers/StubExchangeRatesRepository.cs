using ExchangeRateUpdater.Repositories;

namespace ExchangeRateUpdater.Tests.UnitTests.Helpers;

public class StubExchangeRatesRepository : IExchangeRatesRepository
{
    private readonly ExchangeRate[] _rates;

    public StubExchangeRatesRepository(params ExchangeRate[] rates)
    {
        _rates = rates;
    }

    public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
    {
        return _rates.ToList();
    }
}