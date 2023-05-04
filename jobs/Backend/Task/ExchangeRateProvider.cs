using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider: IExchangeRateProvider
{
    private const string ExchangeRateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={0:dd.MM.yyyy}";
    private readonly HttpClient _httpClient;

    public ExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var sourceData = await FetchExchangeRateDataAsync(DateTime.Today);
        var exchangeRates = ParseExchangeRates(sourceData, currencies);
        return exchangeRates;
    }

    private async Task<string> FetchExchangeRateDataAsync(DateTime date)
    {
        var url = string.Format(ExchangeRateUrl, date);
        
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve exchange rates: {response.StatusCode}");
        }
        return response.Content.ReadAsStringAsync().Result;
    }

    //creates the list of exchange rates will be output.
    private IEnumerable<ExchangeRate> ParseExchangeRates(string sourceData, IEnumerable<Currency> currencies)
    {
        var exchangeRates = new List<ExchangeRate>();
        var currencyCodes = currencies.Select(c => c.Code.ToUpperInvariant()).ToList();
        var lines = sourceData.Split('\n');

        //start at the correct index.
        var startIndex = FindStartingIndex(lines);
        for (int i = startIndex + 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var exchangeRate = ParseExchangeRate(line, currencyCodes);
            if (exchangeRate != null)
            {
                exchangeRates.Add(exchangeRate);
            }
        }

        return exchangeRates;
    }

    //Find the starting index, to avoid parsing the wrong data.
    private int FindStartingIndex(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("Country|Currency|Amount|Code|Rate"))
            {
                return i;
            }
        }
        throw new Exception("Starting index not found in exchange rate data");
    }

    //passes in individual Line , gets the currency code and value if the format is correct.
    private ExchangeRate ParseExchangeRate(string line, List<string> currencyCodes)
    {
        //creates an array of "se
        var parts = line.Split('|');
        if (parts.Length != 5)
        {
            throw new Exception($"Invalid format on line: {line}");
        }
        var code = parts[3].ToUpperInvariant();
        if (!currencyCodes.Contains(code))
        {
            return null;
        }
        decimal value;
        if (!decimal.TryParse(parts[4], out value))
        {
            throw new Exception($"Invalid exchange rate on line: {line}");
        }
        return new ExchangeRate(new Currency(code), new Currency("CZK"), value);
    }
}

