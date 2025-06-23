using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies;
using FluentValidation;
using MediatR;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, IEnumerable<GetExchangeRatesQueryResponse>>
    {
        private readonly IEnumerable<IExchangeRateProviderStrategy> _providerStrategies;

        public GetExchangeRatesQueryHandler(IEnumerable<IExchangeRateProviderStrategy> providerStrategies)
        {
            _providerStrategies = providerStrategies;
        }

        public async Task<IEnumerable<GetExchangeRatesQueryResponse>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var providerStrategy = GetMatchingProviderStrategy(request.ProviderCode);
            return await providerStrategy.GetExchangeRatesAsync(request, cancellationToken);
        }

        private IExchangeRateProviderStrategy GetMatchingProviderStrategy(string providerCode)
        {
            var matchingProviderStrategy = _providerStrategies.FirstOrDefault(x => x.CanHandle(providerCode));
            if (matchingProviderStrategy is null)
            {
                throw new ValidationException($"Provider {providerCode} not supported");
            }

            return matchingProviderStrategy;
        }
    }
}
