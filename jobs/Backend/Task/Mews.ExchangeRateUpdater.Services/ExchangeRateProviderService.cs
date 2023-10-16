using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.DtoMappers;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders;
using Mews.ExchangeRateUpdater.Services.Validators;

namespace Mews.ExchangeRateUpdater.Services
{
    /// <summary>
    /// This is the main service implementation which acts as a facade and communicates with other classes to facilitate the
    /// exchange rate fetch process seamless
    /// </summary>
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IExchangeRateProviderResolver _exchangeRateProviderResolver;
        private readonly IEnumerable<IRequestValidator> _requestValidators;

        public ExchangeRateProviderService(IExchangeRateProviderResolver exchangeRateProviderResolver, IEnumerable<IRequestValidator> requestValidators)
        {
            _exchangeRateProviderResolver = exchangeRateProviderResolver;
            _requestValidators = requestValidators;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(List<CurrencyDto> currencies)
        {
            var validationMessages = new List<string>();

            currencies ??= new List<CurrencyDto>();

            // 1. As a first step validates and sanitises the input if required
            foreach (var requestValidator in _requestValidators)
            {
                validationMessages = requestValidator.Validate(ref currencies, ref validationMessages);
            }

            // 2. Get the appropriate exchange rate provider to use to fetch the rates, this is done like this anticipating
            // the future use of some other provider if needed, so that we can implement a whole new process to integrate
            // with the new provider instead of modifying the existing implementation to cater for the new requirements, in this
            // way we are respecting SRP and OCP of SOLID
            var exchangeRateProvider = _exchangeRateProviderResolver.GetExchangeRateProvider();

            // 3. Call the provider to fetch the exchange rate model collection
            var exchangeRateModelCollection = await exchangeRateProvider.GetExchangeRates();

            // 4. This is to eliminate those requested currency codes, if not present in the provider source of exchange rate collection
            var matchingExchangeRateModelCollection = exchangeRateModelCollection.Join
                (currencies, outerKey => outerKey.SourceCurrency.Code, innerKey => innerKey.Code, (exchangeRateModel, currencyDto) => exchangeRateModel,
                StringComparer.OrdinalIgnoreCase);

            // 5. This is to map the exchange rate model collection to exchange rate dto collection, as we don't need all the fields of exchange
            // rate model and also have to do some sort of calculation to find out the exchange rate for a single amount of currency if in case of
            // source is providing the exchange rate for multiple amount of currency for the same
            var exchangeRateDtoCollection = matchingExchangeRateModelCollection.ToExchangeRateDtos();

            return exchangeRateDtoCollection;
        }
    }
}
