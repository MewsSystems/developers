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

        
        public async Task<IEnumerable<ExchangeRate>> ExecuteAsync(Currency target, IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            try
            {
                Guard.Against.Null(target);
                Guard.Against.NullOrEmpty(currencies);

                var exchangeRates = await _exchangeRateProvider.GetDailyExchangeRates(target, cancellationToken);

                return exchangeRates?.Where(x => currencies.Any(y => y.Code.Equals(x.SourceCurrency.Code))) ?? Enumerable.Empty<ExchangeRate>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when retrieving exchange daily rates");
                throw;
            }

        }
    }
}
