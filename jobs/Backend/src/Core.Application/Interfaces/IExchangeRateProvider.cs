using Core.Domain.Models;
using CSharpFunctionalExtensions;

namespace Core.Application.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<Result<IReadOnlyCollection<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}