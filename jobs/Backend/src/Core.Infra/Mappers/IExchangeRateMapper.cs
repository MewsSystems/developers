using Core.Domain.Models;
using Core.Infra.Dtos;
using CSharpFunctionalExtensions;

namespace Core.Infra.Mappers
{
    public interface IExchangeRateMapper
    {
        Result<IReadOnlyCollection<ExchangeRate>> Map(IReadOnlyCollection<ExchangeRateDto> exchangeSource);
    }
}