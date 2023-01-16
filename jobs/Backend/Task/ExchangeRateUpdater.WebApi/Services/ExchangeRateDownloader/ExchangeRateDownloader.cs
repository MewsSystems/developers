using ExchangeRateUpdater.WebApi.Exceptions;

namespace ExchangeRateUpdater.WebApi.Services.ExchangeRateDownloader;

public class ExchangeRateDownloader : IExchangeRateDownloader
{
    private readonly IConfiguration _configuration;

    public ExchangeRateDownloader(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> DownloadExchangeRates()
    {
        var exchangeRatesSource = _configuration.GetSection("ExchangeRatesSource").Value;

        if (string.IsNullOrEmpty(exchangeRatesSource))
        {
            throw new InvalidConfigurationException( "ExchangeRatesSource invalid configuration");
        }

        return await DownloadExchangeRatesFile(exchangeRatesSource);
        
    }

    private async Task<string> DownloadExchangeRatesFile(string exchangeRatesSource)
    {
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(exchangeRatesSource);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}