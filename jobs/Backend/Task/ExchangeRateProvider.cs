using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;
public class ExchangeRateProvider : IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>

    private readonly string _dailyExchangeProviderUrl;
    private readonly string _monthlyExchangeProviderUrl;
    private readonly HttpClient _httpClient;

    public ExchangeRateProvider(IOptions<ExchangeRateProviderOptions> options)
    {
        _dailyExchangeProviderUrl = options.Value.BaseUrl;
        _monthlyExchangeProviderUrl = options.Value.OtherCurrenciesUrl;
        _httpClient = new HttpClient();
    }
    
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        if (!currencies.Any())
            return Enumerable.Empty<ExchangeRate>();
        
        var dailyExchangeRates = await GetDailyExchanges();
        
        var monthlyExchanges = await GetMonthlyExchanges();
        
        var exchangeRates = new List<ExchangeRate>(dailyExchangeRates);
        
        exchangeRates.AddRange(
            monthlyExchanges.Where(exchange => 
                !dailyExchangeRates.Any(ex => ex.Equals(exchange))));

        return GetSelectedExchangeRates(currencies, exchangeRates);
    }

    protected virtual async Task<IEnumerable<ExchangeRate>> GetDailyExchanges()
    {
        var dailyFileName = $"daily_exchange_{DateTime.Now:dd-MM-yyyy}.txt";
        var filePath = await GetFile(dailyFileName, _dailyExchangeProviderUrl);
        return await ReadFileLineByLineAsync(filePath);
    }

    protected virtual async Task<IEnumerable<ExchangeRate>> GetMonthlyExchanges()
    {
        var monthlyFileName = $"monthly_exchange_{DateTime.Now.Month}-{DateTime.Now.Year}.txt";
        var filePath = await GetFile(monthlyFileName, _monthlyExchangeProviderUrl);
        return await ReadFileLineByLineAsync(filePath);
    }

    private async Task<string> GetFile(string fileName, string url)
    {
        var filePath = Path.Combine(Path.GetTempPath(), fileName);
        
        if(File.Exists(filePath))
            return filePath;
        
        var response = await _httpClient.GetAsync(url);
        
        if(!response.IsSuccessStatusCode)
            throw new ApplicationException($"Unable to get exchange rate from url: {url}");
        
        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        
        await stream.CopyToAsync(fileStream);
        
        return filePath;
    }
    
    private static async Task<IEnumerable<ExchangeRate>> ReadFileLineByLineAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to read exchange rate from file: {filePath}");
        }
        
        var exchangeRates = new List<ExchangeRate>();
        
        using var streamReader = new StreamReader(filePath);
        string line;
        
        //Remove header and date
        await streamReader.ReadLineAsync();
        await streamReader.ReadLineAsync();

        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            var lineSplitted = line.Split('|');
            
            var sourceCurrencyString = lineSplitted[3];
            
            var quantity = decimal.Parse(lineSplitted[2]);
            var rate = decimal.Parse(lineSplitted[4], System.Globalization.CultureInfo.InvariantCulture);
            
            var targetCurrency = new Currency("CZK");
            var sourceCurrency = new Currency(sourceCurrencyString);
            
            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate/quantity));
        }

        return exchangeRates;
    }

    private IEnumerable<ExchangeRate> GetSelectedExchangeRates(IEnumerable<Currency> currencies, IEnumerable<ExchangeRate> exchanges)
    {
        return currencies.Select(currency => 
                exchanges.FirstOrDefault(exchange => exchange.SourceCurrency.Code == currency.Code))
            .Where(exchange => exchange != null)
            .ToList();
    }
}