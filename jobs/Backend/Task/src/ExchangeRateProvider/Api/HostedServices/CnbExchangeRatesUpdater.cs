using ExchangeRateUpdater.Common.Constants;
using ExchangeRateUpdater.Infrastructure.Options;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace ExchangeRateUpdater.Api.HostedServices;

public class CnbExchangeRatesUpdater(
    ILogger<CnbExchangeRatesUpdater> logger,
    IServiceProvider serviceProvider,
    IFeatureManager featureManager,
    IOptions<CnbExchangeRatesUpdaterOptions> hostedServiceOptions)
    : BackgroundService
{
    private readonly ILogger<CnbExchangeRatesUpdater> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IFeatureManager _featureManager = featureManager;
    private readonly CnbExchangeRatesUpdaterOptions _exchangeRatesUpdaterOptions = hostedServiceOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.IsExchangeRatesUpdaterHostedServiceEnabled))
        {
            _logger.LogInformation("{HostedServiceName} is disabled", nameof(CnbExchangeRatesUpdater));
            return;
        }

        TimeSpan period = TimeSpan.FromSeconds(_exchangeRatesUpdaterOptions.RetryOnRefreshFailureInSeconds!.Value);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                var czechNationalBankService = scope.ServiceProvider.GetRequiredService<ICzechNationalBankService>();
                var exchangeRates = await czechNationalBankService.RefreshExchangeRatesAsync();

                if (exchangeRates is not null && exchangeRates.Any())
                {
                    period = DateTime.UtcNow.Date.AddDays(1) + _exchangeRatesUpdaterOptions.DailyRefreshUtcTime!.Value.ToTimeSpan() - DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing {HostedServiceName}", nameof(CnbExchangeRatesUpdater));
                period = TimeSpan.FromSeconds(_exchangeRatesUpdaterOptions.RetryOnRefreshFailureInSeconds!.Value);
            }

            await Task.Delay(period, stoppingToken);
        }
    }
}
