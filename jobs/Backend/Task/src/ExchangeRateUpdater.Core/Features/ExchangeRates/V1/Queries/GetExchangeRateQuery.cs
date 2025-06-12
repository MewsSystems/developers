using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Interfaces;
using MediatR;
using NodaTime;
using NodaTime.Text;

namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;

public record GetExchangeRateQuery(
    string SourceCurrency,
    string TargetCurrency,
    string? Date = null) : IRequest<ExchangeRateResponse>;

public class GetExchangeRateQueryHandler : IRequestHandler<GetExchangeRateQuery, ExchangeRateResponse>
{
    private readonly IExchangeRateService _exchangeRateService;

    public GetExchangeRateQueryHandler(
        IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<ExchangeRateResponse> Handle(GetExchangeRateQuery request,
        CancellationToken cancellationToken)
    {
        LocalDate? date = null;

        if (!string.IsNullOrWhiteSpace(request.Date))
        {
            var parseResult = LocalDatePattern.Iso.Parse(request.Date);
            if (!parseResult.Success) throw new ArgumentException($"Invalid date format: {request.Date}. Date must be in ISO-8601 format (yyyy-MM-dd).", nameof(request.Date));
            date = parseResult.Value;
        }

        return await _exchangeRateService.GetExchangeRateAsync(
            request.SourceCurrency,
            request.TargetCurrency,
            date,
            cancellationToken);
    }
}