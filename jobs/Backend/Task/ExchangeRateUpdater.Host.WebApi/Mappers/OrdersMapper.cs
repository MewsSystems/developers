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
    /// <param name="exchangeOrderDto">Instance of <see cref="ExchangeOrderDto"/> class.</param>
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
    /// <param name="exchangeResult">Instance of <see cref="ExchangeResult"/> class.</param>
    /// <returns>Returns an instance of ExchangeResultDto.</returns>
    internal static ExchangeResultDto ToExchangeResultDto(this ExchangeResult exchangeResult)
    {
        return new ExchangeResultDto
        {
            SourceCurrency = exchangeResult.SourceCurrency,
            TargetCurrency = exchangeResult.TargetCurrency,
            ConvertedSum = exchangeResult.ConvertedSum,
            ExchangeRateDate = exchangeResult.RateDate
        };
    }
}
