using MediatR;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class GetExchangeRatesQuery : IRequest<GetExchangeRatesResponse>
{
    public List<string>? CurrencyCodes { get; init; }
    public DateOnly? Date { get; init; }
}