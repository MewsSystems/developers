using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Host.WebApi.Dtos.Response;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

internal static class OrderResultMapper
{
    internal static BuyResultDto ToBuyResultDto(this BuyResult buyResult)
    {
        return new BuyResultDto
        {
            SourceCurrency = buyResult.SourceCurrency,
            TargetCurrency = buyResult.TargetCurrency,
            ConvertedSum   = buyResult.ConvertedSum
        };
    }
}
