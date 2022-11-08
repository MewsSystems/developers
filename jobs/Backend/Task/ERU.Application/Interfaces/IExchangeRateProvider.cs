using ERU.Domain;

namespace ERU.Application.Interfaces;

public interface IExchangeRateProvider
{
	Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken token = default);
}