using Ardalis.GuardClauses;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.UseCases
{
    public class GetDailyExchangeRateUseCase : IGetDailyExchangeRateUseCase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ILogger<GetDailyExchangeRateUseCase> _logger;

        public GetDailyExchangeRateUseCase(IExchangeRateProvider exchangeRateProvider, ILogger<GetDailyExchangeRateUseCase> logger)
        {
            _exchangeRateProvider = Guard.Against.Null(exchangeRateProvider);
            _logger = Guard.Against.Null(logger);
        }

        
        public async Task<IEnumerable<ExchangeRate>> ExecuteAsync(Currency source, IEnumerable<Currency> targetCurrencies, CancellationToken cancellationToken)
        {
            try
            {
                Guard.Against.Null(source);
                Guard.Against.NullOrEmpty(targetCurrencies);

                var exchangeRates = await _exchangeRateProvider.GetDailyExchangeRates(source, cancellationToken);

                return exchangeRates?.Where(x => targetCurrencies.Any(y => y.Code.Equals(x.TargetCurrency.Code))) ?? Enumerable.Empty<ExchangeRate>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when retrieving exchange daily rates");
                throw;
            }

        }
    }
}
