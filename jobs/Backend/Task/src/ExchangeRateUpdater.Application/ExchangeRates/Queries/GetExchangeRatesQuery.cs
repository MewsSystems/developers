using AutoMapper;
using ExchangeRateUpdater.Application.Common.Dto;
using ExchangeRateUpdater.Application.ExchangeRates.Dto;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.ValueObjects;
using MediatR;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries
{
    /// <summary>
    /// Represents a query to get exchange rates.
    /// </summary>
    public record GetExchangeRatesQuery : IRequest<ListResponse<ExchangeRateDto>>
    {
        /// <summary>
        /// List of three-letter ISO 4217 currency codes for which exchange rates are requested.
        /// If empty, all available rates are fetched.
        /// </summary>
        public List<string> CurrencyCodes { get; set; } = [];

        /// <summary>
        /// Date for which exchange rates are requested.
        /// If null, the latest available rates are fetched.
        /// </summary>
        public DateOnly? Date { get; set; }
    }

    /// <summary>
    /// Handles the <see cref="GetExchangeRatesQuery"/> and retrieves exchange rates.
    /// </summary>
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, ListResponse<ExchangeRateDto>>
    {
        private readonly IMapper _mapper;
        private readonly IExchangeRateProvider _exchangeRateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExchangeRatesQueryHandler"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="exchangeRateProvider">The exchange rate provider for retrieving exchange rates.</param>
        public GetExchangeRatesQueryHandler(IMapper mapper, IExchangeRateProvider exchangeRateProvider)
        {
            _mapper = mapper;
            _exchangeRateProvider = exchangeRateProvider;
        }

        /// <inheritdoc />
        public async Task<ListResponse<ExchangeRateDto>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var currencies = request.CurrencyCodes.Select(x => new Currency(x));
            var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(request.Date, currencies);

            return new ListResponse<ExchangeRateDto>() { Data = _mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates).ToList() };
        }
    }
}
