using AutoMapper;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi;
using ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Queries;
using Polly;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a repository for retrieving exchange rates from the Czech National Bank API.
    /// </summary>
    internal class ExchangeRatesRepository : IExchangeRatesRepository
    {
        private readonly IMapper _mapper;
        private readonly ICzechNationalBankApi _cnbApi;

        private const int RetryCount = 3;
        private const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRatesRepository"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="cnbApi">The Czech National Bank API client.</param>
        public ExchangeRatesRepository(IMapper mapper, ICzechNationalBankApi cnbApi)
        {
            _mapper = mapper;
            _cnbApi = cnbApi;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateOnly? date)
        {
            var query = new ExchangeRatesQuery(Language.EN, date?.ToString(DateFormat));

            var retryPolicy = Policy
                .Handle<ApiException>()
                .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var response = await retryPolicy.ExecuteAsync(async () => await _cnbApi.GetExchangeRatesAsync(query));
            return _mapper.Map<IEnumerable<ExchangeRate>>(response.Rates);
        }
    }
}
