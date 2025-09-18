using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Services.Interfaces;

public interface IExchangeRateParser
{
    Task<IEnumerable<ExchangeRate>> ParseAsync(string xmlContent);
}