using AutoMapper;
using ExchangeRateUpdater.Implementation.Queries;
using ExchangeRateUpdater.Interface.DTOs;
using FluentValidation;
using Grpc.Core;
using MediatR;

namespace ExchangeRateUpdater.Grpc.Services
{
    public class ExchangeRatesService : Grpc.ExchangeRatesService.ExchangeRatesServiceBase
    {
        private readonly ILogger<ExchangeRatesService> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IValidator<GetExchangeRatesQuery> _getExchangeRatesQueryValidator;
        private readonly IValidator<CurrencyDto> _currencyValidator;

        public ExchangeRatesService(ILogger<ExchangeRatesService> logger, IMediator mediator, IMapper mapper, IValidator<GetExchangeRatesQuery> getExchangeRatesQueryValidator,
            IValidator<CurrencyDto> currencyValidator)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _getExchangeRatesQueryValidator = getExchangeRatesQueryValidator;
            _currencyValidator = currencyValidator;
        }

        public override async Task<GetExchangeRatesResponse> GetExchangeRates(GetExchangeRatesRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Number of currency rates to find: {request.Currencies.Count}");

            var query = new GetExchangeRatesQuery { Currencies = _mapper.Map<IEnumerable<CurrencyDto>>(request.Currencies) };

            ValidateQuery(query);

            var exchangeRatesDto = await _mediator.Send(query);

            _logger.LogInformation($"Number of currency rates found: {exchangeRatesDto?.Count()}");

            var response = new GetExchangeRatesResponse();
            response.ExchangeRates.AddRange(_mapper.Map<IEnumerable<ExchangeRate>>(exchangeRatesDto));

            return response;
        }

        private void ValidateQuery(GetExchangeRatesQuery query)
        {
            _getExchangeRatesQueryValidator.ValidateAndThrow(query);

            foreach (var currency in query.Currencies)
            {
                _currencyValidator.ValidateAndThrow(currency);
            }
        }
    }
}