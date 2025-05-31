using Microsoft.Extensions.Caching.Distributed;
using Services.Models.CzechNationalBankApi;

namespace Services;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
}

internal class CzechCrownRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankClient _czechNationalBankClient;
    private readonly IDistributedCache _distributedCache;

    public CzechCrownRateProvider(ICzechNationalBankClient czechNationalBankClient, IDistributedCache distributedCache)
    {
        _czechNationalBankClient = czechNationalBankClient;
        _distributedCache = distributedCache;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var rates = await _czechNationalBankClient.GetExchangeRatesAsync();
        
        return Enumerable.Empty<ExchangeRate>();
    }
}

public interface ICzechNationalBankClient
{
    Task<CzkExchangeRateResponse> GetExchangeRatesAsync();
    Task<CzkExchangeRateResponse> GetOtherExchangeRatesAsync();
}

internal class CzechNationalBankClient : ICzechNationalBankClient
{
    public CzechNationalBankClient(HttpClient client)
    {
    }

    public async Task<CzkExchangeRateResponse> GetExchangeRatesAsync()
    {
        return new CzkExchangeRateResponse()
        {
            Rates = []
        };
    }

    public async Task<CzkExchangeRateResponse> GetOtherExchangeRatesAsync()
    {
        return new CzkExchangeRateResponse()
        {
            Rates = []
        };
    }
}