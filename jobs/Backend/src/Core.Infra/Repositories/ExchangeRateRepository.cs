using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.Infra.Interfaces;
using Core.Infra.Mappers;
using CSharpFunctionalExtensions;

namespace Core.Infra.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly IExchangeRateClient _exchangeRateClient;
        private readonly IExchangeRateDtoMapper _exchangeRateDtoMapper;
        private readonly IExchangeRateMapper _exchangeRateMapper;

        public ExchangeRateRepository(IExchangeRateClient exchangeRateClient,
            IExchangeRateDtoMapper exchangeRateDtoMapper,
            IExchangeRateMapper exchangeRateMapper)
        {
            _exchangeRateClient = exchangeRateClient;
            _exchangeRateDtoMapper = exchangeRateDtoMapper;
            _exchangeRateMapper = exchangeRateMapper;
        }

        public async Task<Result<Maybe<IReadOnlyCollection<ExchangeRate>>>> GetExchangeRates()
        {
            var response = await _exchangeRateClient.GetExchangeRates();

            var ratesDto = _exchangeRateDtoMapper.Map(response);
            if (ratesDto.IsFailure)
            {
                return Result.Failure<Maybe<IReadOnlyCollection<ExchangeRate>>>(Errors.Errors.FailedToRetrieveExchangeRates);
            }

            var rates = _exchangeRateMapper.Map(ratesDto.Value);
            if (rates.IsFailure)
            {
                return Result.Failure<Maybe<IReadOnlyCollection<ExchangeRate>>>(Errors.Errors.FailedToRetrieveExchangeRates);
            }

            return Result.Success(Maybe<IReadOnlyCollection<ExchangeRate>>.From(rates.Value));
        }
    }
}
