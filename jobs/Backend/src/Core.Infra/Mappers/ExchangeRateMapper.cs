using Core.Domain.Models;
using Core.Extensions;
using Core.Infra.Dtos;
using CSharpFunctionalExtensions;

namespace Core.Infra.Mappers
{
    public class ExchangeRateMapper : IExchangeRateMapper
    {
        public Result<IReadOnlyCollection<ExchangeRate>> Map(IReadOnlyCollection<ExchangeRateDto> sourceExchangeRates)
        {
            if (sourceExchangeRates == null)
            {
                return Result.Success<IReadOnlyCollection<ExchangeRate>>(Array.Empty<ExchangeRate>());
            }

            return Result.Success(sourceExchangeRates
                .Select(se => Map(se, new ExchangeRateDto("Czechia", "kroon", "1", "CZK", "1")))
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList().ToReadOnlyCollection());
        }

        private Result<ExchangeRate> Map(ExchangeRateDto sourceRate, ExchangeRateDto targetRate)
        {
            if (sourceRate == null)
            {
                return Result.Failure<ExchangeRate>($"Missing information: {nameof(sourceRate)}.");
            }

            if (targetRate == null)
            {
                return Result.Failure<ExchangeRate>($"Missing information: {nameof(targetRate)}.");
            }

            var sourceCurrency = Currency.Create(sourceRate.Code);
            var targetCurrency = Currency.Create(targetRate.Code);

            return Result.Combine(sourceCurrency, targetCurrency)
                .Bind(() => ExchangeRate.Create(sourceCurrency.Value,
                    targetCurrency.Value,
                    sourceRate.Rate,
                    sourceRate.BaseAmount));
        }
    }
}
