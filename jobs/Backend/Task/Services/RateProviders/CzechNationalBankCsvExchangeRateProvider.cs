using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services.RateProviders;

/// <summary>
///     Gets exchange rates from a CSV file provided by the Czech National Bank (daily.txt file)
/// </summary>
public class CzechNationalBankCsvExchangeRateProvider : IExchangeRateProvider
{
    private const string DateFormat = "dd MMM yyyy";
    private readonly AppConfiguration _appConfiguration;
    private readonly ILogger<CzechNationalBankCsvExchangeRateProvider> _logger;

    public CzechNationalBankCsvExchangeRateProvider(ILogger<CzechNationalBankCsvExchangeRateProvider> logger,
        AppConfiguration appConfiguration)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        using var httpClient = new HttpClient();

        _logger.LogDebug("Fetching exchange rates from {Url}", _appConfiguration.DailyRateUrl);
        var response = await httpClient.GetStringAsync(_appConfiguration.DailyRateUrl);

        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Trim())
            .ToArray();

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
            _logger.LogDebug("Processing line: {Line}", line);

            var parts = line.Split('|');
            if (parts.Length != 5) continue;

            var code = parts[3];

            if (!currencyCodesToFilter.Contains(code))
            {
                _logger.LogInformation("Currency {Code} is not in the requested list, skipping.", code);
                continue;
            }

            var amount = int.Parse(parts[2]);
            var rate = decimal.Parse(parts[4], CultureInfo.InvariantCulture);
            var sourceCurrency = new Currency(code);
            var targetCurrency = new Currency(_appConfiguration.CzkCurrencyCode);
            var normalizedRate = rate / amount; // Normalize to 1 unit of source currency to avoid amount discrepancies

            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, rateDate, normalizedRate);
            _logger.LogDebug("Adding exchange rate: {ExchangeRate}", exchangeRate);
            exchangeRates.Add(exchangeRate);
        }

        return exchangeRates;
    }

    private DateTime GetExtractionDate(string[] lines)
    {
        var dateString = lines[0].Split('#')[0].Trim();
        var rateDate = DateTime.ParseExact(dateString, DateFormat,
            CultureInfo.InvariantCulture);
        return rateDate;
    }
}