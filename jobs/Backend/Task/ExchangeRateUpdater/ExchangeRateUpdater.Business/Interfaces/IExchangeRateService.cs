using ExchangeRateUpdater.Data.Responses;
using ExchangeRateUpdater.Models.Requests;

namespace ExchangeRateUpdater.Business.Interfaces;
public interface IExchangeRateService
{
    Task<List<ExchangeRateResultDto>> GetExchangeRates(IEnumerable<ExchangeRateRequest> currencies, DateTime date, CancellationToken cancellationToken);
}
