using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;

namespace ExchangeRateUpdater.Infrastructure.Services;

public interface ICzechNationalBankService
{
    Task<IEnumerable<CzechNationalBankExchangeRate>> ProvideExchangeRatesAsync();
    Task<IEnumerable<CzechNationalBankExchangeRate>> RefreshExchangeRatesAsync();
}

public class CzechNationalBankService(
    ILogger<CzechNationalBankService> logger,
    ICzechNationalBankApiClient czechNationalBankApiClient,
    ICzechNationalBankCacheAccessor czechNationalBankCacheAccessor)
    : ICzechNationalBankService
{
    private readonly ILogger<CzechNationalBankService> _logger = logger;
    private readonly ICzechNationalBankApiClient _czechNationalBankApiClient = czechNationalBankApiClient;
    private readonly ICzechNationalBankCacheAccessor _czechNationalBankCacheAccessor = czechNationalBankCacheAccessor;

    public async Task<IEnumerable<CzechNationalBankExchangeRate>> ProvideExchangeRatesAsync()
    {
        var cachedExchangeRates = await _czechNationalBankCacheAccessor.GetAsync();
        if (cachedExchangeRates is not null)
        {
            return cachedExchangeRates;
        }

        return await RefreshExchangeRatesAsync();
    }

    public async Task<IEnumerable<CzechNationalBankExchangeRate>> RefreshExchangeRatesAsync()
    {
        string date = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
        var latestCnbExchangeRates = await _czechNationalBankApiClient.GetExchangeRatesAsync(date);
        await _czechNationalBankCacheAccessor.SetAsync(latestCnbExchangeRates.Rates);

        return latestCnbExchangeRates.Rates;
    }
}
