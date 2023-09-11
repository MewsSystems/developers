using Mews.ERP.AppService.Data.Models;
using Mews.ERP.AppService.Data.Repositories;
using Mews.ERP.AppService.Features.Fetch.Models;
using Mews.ERP.AppService.Features.Fetch.Repositories.Interfaces;

namespace Mews.ERP.AppService.Features.Fetch.Repositories;

public class CurrenciesRepository : ReadOnlyRepository<Currency>, ICurrenciesRepository
{
    private readonly List<Currency> internalCurrencies = new()
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };
    
    public override Task<IQueryable<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {

        return Task.FromResult(internalCurrencies.AsQueryable());
    }
}