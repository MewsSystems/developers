using ExchangeRateUpdater.WebApi.Exceptions;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateDownloader;

namespace ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;

public class ExchangeRateParser : IExchangeRateParser
{
    private readonly IExchangeRateDownloader _exchangeRateDownloader;
    private readonly IConfiguration _configuration;

    private const int ExchangeRateLineLength = 5;

    public ExchangeRateParser(IConfiguration configuration, IExchangeRateDownloader exchangeRateDownloader)
    {
        _configuration = configuration;
        _exchangeRateDownloader = exchangeRateDownloader;
    }

    public async Task<IEnumerable<ExchangeRate>> ParseExchangeRates()
    {
        var sourceCurrency = _configuration.GetSection("SourceCurrency").Value;

        if (string.IsNullOrEmpty(sourceCurrency))
        {
            throw new InvalidConfigurationException("Invalid source currency configuration");
        }

        string downloadedExchangeRates;

        try
        {
            downloadedExchangeRates = await _exchangeRateDownloader.DownloadExchangeRates();
        }
        catch (HttpRequestException)
        {
            var exchangeRatesSource = _configuration.GetSection("ExchangeRatesSource").Value;
            throw new ServiceUnavailableException($"Exchange rates source {exchangeRatesSource} is not responding");
        }
        
        if (string.IsNullOrEmpty(downloadedExchangeRates))
        {
            return Array.Empty<ExchangeRate>();
        }

        var downloadedExchangeRateLines = downloadedExchangeRates.Contains(Environment.NewLine)? downloadedExchangeRates.Split(Environment.NewLine): 
                downloadedExchangeRates.Split('\n');
        
        var exchangeRatesAvailable = downloadedExchangeRateLines.Skip(2);

        return exchangeRatesAvailable.
            Select(exchangeRateLine => exchangeRateLine.Split('|')).
            Where(exchangeRateLine => exchangeRateLine.Length >= ExchangeRateLineLength && decimal.TryParse(exchangeRateLine[4], out _)).
            Select(exchangeRateLineSplitted => 
            new ExchangeRate(new Currency(sourceCurrency), new Currency(exchangeRateLineSplitted[3]), decimal.Parse(exchangeRateLineSplitted[4]))).ToArray();

    }

 }