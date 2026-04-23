using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

internal sealed class HostedService : IHostedService
{
    private readonly ILogger<HostedService> _logger;
    private readonly IOptions<HostedServiceConfiguration> _configuration;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public HostedService(ILogger<HostedService> logger, IOptions<HostedServiceConfiguration> configuration,
        IExchangeRateProvider exchangeRateProvider)
    {
        _logger = logger ??
            throw new ArgumentNullException(nameof(logger));

        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));

        _exchangeRateProvider = exchangeRateProvider ??
            throw new ArgumentNullException(nameof(exchangeRateProvider));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await GetExchangeRatesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not retrieve exchange rates.");
        }
    }

    async Task GetExchangeRatesAsync()
    {
        IEnumerable<Currency> currencies = _configuration.Value.TargetCurrencies
            .Select(currencyCode => new Currency(currencyCode))
            .ToList();

        IEnumerable<ExchangeRate> rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

        _logger.LogInformation("Successfully retrieved {count} exchange rates.", rates.Count());

        foreach (var rate in rates)
        {
            _logger.LogInformation("{rate}", rate.ToString());
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}