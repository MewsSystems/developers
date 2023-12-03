using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Host.WebApi.Dtos.Response;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

internal static class OrderResultMapper
{
    internal static ExchangeResultDto ToBuyResultDto(this ExchangeResult buyResult)
    {
        return new ExchangeResultDto
        {
            SourceCurrency = buyResult.SourceCurrency,
            TargetCurrency = buyResult.TargetCurrency,
            ConvertedSum   = buyResult.ConvertedSum
        };
    }
}
