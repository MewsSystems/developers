using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRates;

public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, IEnumerable<ExchangeRate>>
{
    private readonly IExchangeRateRepository _repository;

    public GetExchangeRatesQueryHandler(IExchangeRateRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExchangeRate>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetExchangeRatesAsync(request.Day, cancellationToken);
    }
}
