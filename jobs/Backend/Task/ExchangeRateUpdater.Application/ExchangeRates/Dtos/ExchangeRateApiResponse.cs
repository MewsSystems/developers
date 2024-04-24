namespace ExchangeRateUpdater.Application.ExchangeRates.Dtos;

using Query;

public class ExchangeRateApiResponse
{
    public ExchangeRateApiDto[] Rates { get; set; }
}