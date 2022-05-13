using Core.Infra.Dtos;
using CSharpFunctionalExtensions;

namespace Core.Infra.Mappers
{
    public interface IExchangeRateDtoMapper
    {
        Result<IReadOnlyCollection<ExchangeRateDto>> Map(string exchangeSource);
    }
}
