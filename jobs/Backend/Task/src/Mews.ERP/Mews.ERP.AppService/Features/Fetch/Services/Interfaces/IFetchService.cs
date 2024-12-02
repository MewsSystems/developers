using Mews.ERP.AppService.Features.Fetch.Models;

namespace Mews.ERP.AppService.Features.Fetch.Services.Interfaces;

public interface IFetchService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
}