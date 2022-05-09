using Core.Domain.Models;
using CSharpFunctionalExtensions;

namespace Core.Domain.Interfaces
{
    public interface IExchangeRateRepository
    {
        public Task<Result<Maybe<IReadOnlyCollection<ExchangeRate>>>> GetExchangeRates();
    }
}