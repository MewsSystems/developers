using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Dtos.Request;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

internal static class OrdersMapper
{
    internal static BuyOrder ToOrderBuy(this BuyOrderDto orderBuyDto)
    {
        return new BuyOrder(
            new Currency(orderBuyDto.SourceCurrency),
            new Currency(orderBuyDto.TargetCurrency),
            new PositiveRealNumber(orderBuyDto.SumToExchange!.Value));
    }
}
