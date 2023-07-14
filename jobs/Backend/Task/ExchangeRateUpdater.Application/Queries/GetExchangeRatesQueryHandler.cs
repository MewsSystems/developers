using ExchangeRateUpdater.Domain.Model;
using ExchangeRateUpdater.Interface.DTOs;
using ExchangeRateUpdater.Interface.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Implementation.Queries
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, IEnumerable<ExchangeRateDto>>
    {
        private ILogger<GetExchangeRatesQueryHandler> _logger;
        private readonly IExchangeRatesCacheService _exchangeRatesCacheService;

        public GetExchangeRatesQueryHandler(ILogger<GetExchangeRatesQueryHandler> logger, IExchangeRatesCacheService exchangeRatesCacheService)
        {
            _logger = logger;
            _exchangeRatesCacheService = exchangeRatesCacheService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRateDto>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            ExchangeRateEntity? exchangeRate;
            ExchangeRateDto? exchangeRateDto;
            var result = new List<ExchangeRateDto>();

            var exchangeRatesList = await _exchangeRatesCacheService.GetOrCreateExchangeRatesAsync();

            foreach (var currency in request.Currencies)
            {
                exchangeRate = exchangeRatesList?.Where(x => x.CurrencyCode is not null && x.CurrencyCode.Equals(currency.Code)).FirstOrDefault();

                if (exchangeRate != null)
                {
                    exchangeRateDto = new ExchangeRateDto
                    {
                        TargetCurrency = new CurrencyDto
                        {
                            Code = exchangeRate.CurrencyCode
                        },
                        Value = exchangeRate.Rate * exchangeRate.Amount
                    };

                    result.Add(exchangeRateDto);

                    _logger.LogInformation($"Exchange rate found: {exchangeRateDto}");
                }
                else
                {
                    _logger.LogError($"A currency with ISO 4217 code '{currency.Code}' could not be found.");
                }
            }
            
            return result;
        }
    }
}
