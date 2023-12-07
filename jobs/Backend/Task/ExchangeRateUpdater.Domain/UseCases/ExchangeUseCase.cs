using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using Serilog;

namespace ExchangeRateUpdater.Domain.UseCases;

/// <summary>
/// The use ase that performs the exchange/
/// </summary>
public class ExchangeUseCase
{
    private ILogger _logger;
    private IExchangeRateProviderRepository _exchangeRateProviderRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exchangeRateProviderRepository">Adapter used to get the exchange rates.</param>
    /// <param name="logger">Serilog Logger instance</param>
    /// <exception cref="ArgumentNullException">both exchangeRateProviderRepository and logger can't be null.</exception>
    public ExchangeUseCase(IExchangeRateProviderRepository exchangeRateProviderRepository, ILogger logger)
    {
        _exchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Executes an exchange order.
    /// </summary>
    /// <param name="exchangeOrder">The order on which to make the exchange.</param>
    /// <param name="requestDate">The specified date valid for the exchange.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
    /// <returns>Result of the exchange operation.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<ExchangeResult?> ExecuteAsync(ExchangeOrder exchangeOrder, DateTime requestDate, CancellationToken cancellationToken)
    {
        if (exchangeOrder == null) throw new ArgumentNullException(nameof(exchangeOrder));

        var latestExchange = await GetLatestChangeRateAsync(exchangeOrder, requestDate, cancellationToken);
        if (latestExchange == null)
        {
            _logger.Warning("Could not retrieve latest exchange rate for currencies {SourceCurrency}/{TargetCurrency}", exchangeOrder.SourceCurrency, exchangeOrder.TargetCurrency);
            return null;
        }
        var convertedSum = new PositiveRealNumber(exchangeOrder.SumToExchange * latestExchange.CurrencyRate);

        return new ExchangeResult(exchangeOrder.SourceCurrency, exchangeOrder.TargetCurrency, convertedSum);
    }

    /// <summary>
    /// Sometimes when you query for a date we can get no rates for that day. If the interval is too small, it would be nice
    /// to try to expand it a bit and try to get the latest available exchange rate.
    /// </summary>
    /// <param name="requestDate">The request date.</param>
    /// <returns>The array of retryable time intervals.</returns>
    private DateTime[] GetRetryableStartOfIntervals(DateTime requestDate)
    {
        return new[]
        {
            requestDate,
            requestDate.AddDays(-1),
            requestDate.AddDays(-7),
            requestDate.AddDays(-30),
        };
    }


    /// <summary>
    /// Get the latest exchange rate.
    /// </summary>
    /// <param name="exchangeOrder">Order to be exchanged.</param>
    /// <param name="requestDate">Date the exchange has to happen on.</param>
    /// <param name="cancellationToken">CancellationToken instance.</param>
    /// <returns></returns>
    private async Task<ExchangeRate?> GetLatestChangeRateAsync(ExchangeOrder exchangeOrder, DateTime requestDate, CancellationToken cancellationToken)
    {
        foreach (var beginDate in GetRetryableStartOfIntervals(requestDate))
        {
            var exchangeRates = await _exchangeRateProviderRepository
                       .GetExchangeRateForCurrenciesAsync(exchangeOrder.SourceCurrency, 
                                                          exchangeOrder.TargetCurrency, 
                                                          beginDate.Date, 
                                                          requestDate.Date,
                                                          cancellationToken);

            if (exchangeRates.Any()) return exchangeRates.First();
        }

        return null;
    }
}
