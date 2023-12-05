using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Dtos.Request;
using ExchangeRateUpdater.Host.WebApi.Dtos.Response;

namespace ExchangeRateUpdater.Host.WebApi.Mappers;

/// <summary>
/// Static class responsible for mapping to dto of exchange orders functionality.
/// </summary>
internal static class OrdersMapper
{
    /// <summary>
    /// Maps an ExchangeOrderDto class to ExchangeOrder.
    /// </summary>
    /// <param name="exchangeRate">Instance of ExchangeOrderDto class.</param>
    /// <returns>Returns an instance of ExchangeOrder.</returns>
    internal static ExchangeOrder ToExchange(this ExchangeOrderDto exchangeOrderDto)
    {
        return new ExchangeOrder(
            new Currency(exchangeOrderDto.SourceCurrency),
            new Currency(exchangeOrderDto.TargetCurrency),
            new PositiveRealNumber(exchangeOrderDto.SumToExchange!.Value));
    }


    /// <summary>
    /// Maps an ExchangeResult class to ExchangeResultDto.
    /// </summary>
    /// <param name="exchangeRate">Instance of ExchangeResult class.</param>
    /// <returns>Returns an instance of ExchangeResultDto.</returns>
    internal static ExchangeResultDto ToExchangeResultDto(this ExchangeResult buyResult)
    {
        return new ExchangeResultDto
        {
            SourceCurrency = buyResult.SourceCurrency,
            TargetCurrency = buyResult.TargetCurrency,
            ConvertedSum = buyResult.ConvertedSum
        };
    }
}
