using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.config;
using ExchangeRateUpdater.model;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.services;

/// <summary>
/// Gets exchange rates from a CSV file provided by the Czech National Bank
/// </summary>
public class CzechNationalBankCsvExchangeRateProvider : IExchangeRateProvider
{
    private readonly AppConfiguration _appConfiguration;
    private readonly ILogger<CzechNationalBankCsvExchangeRateProvider> _logger;
    
    public CzechNationalBankCsvExchangeRateProvider(ILogger<CzechNationalBankCsvExchangeRateProvider> logger, AppConfiguration appConfiguration)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
    }
    
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(_appConfiguration.DailyRateUrl);

        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var rateDate = GetExtractionDate(lines);

        var currencyCodesToFilter = new HashSet<string>(
            currencies.Select(c => c.Code),
            StringComparer.OrdinalIgnoreCase);

        var exchangeRates = new List<ExchangeRate>();
        // Filter out the first 2 lines as they are headers and date info
        // Example:
        // 01 Oct 2025 #191
        // Country|Currency|Amount|Code|Rate
        foreach (var line in lines.Skip(2))
        {
            var parts = line.Split('|');
            if (parts.Length != 5) continue;

            var code = parts[3];

            if (!currencyCodesToFilter.Contains(code))
            {
                _logger.LogInformation("Currency {Code} not in the requested list, skipping.", code);
                continue;
            }

            var amount = int.Parse(parts[2]);
            var rate = decimal.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture);

            var sourceCurrency = new Currency(code);
            var targetCurrency = new Currency("CZK");

            var normalizedRate = rate / amount;

            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rateDate, normalizedRate));
        }

        return exchangeRates;
    }

    private DateTime GetExtractionDate(string[] lines)
    {
        var dateString = lines[0].Split('#')[0].Trim();
        var rateDate = DateTime.ParseExact(dateString, "dd MMM yyyy", 
            System.Globalization.CultureInfo.InvariantCulture);
        return rateDate;
    }
}