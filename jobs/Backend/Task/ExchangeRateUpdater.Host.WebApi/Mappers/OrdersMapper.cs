using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Dtos.Request;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

internal static class OrdersMapper
{
    internal static ExchangeOrder ToOrderBuy(this ExchangeOrderDto exchangeOrderDto)
    {
        return new ExchangeOrder(
            new Currency(exchangeOrderDto.SourceCurrency),
            new Currency(exchangeOrderDto.TargetCurrency),
            new PositiveRealNumber(exchangeOrderDto.SumToExchange!.Value));
    }
}
