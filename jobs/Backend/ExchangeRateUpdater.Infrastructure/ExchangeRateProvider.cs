using CsvHelper.Configuration;
using CsvHelper;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Provides exchange rates from the Czech National Bank (CNB) API.
/// Implements <see cref="IExchangeRateProvider"/> to fetch and parse currency exchange rates.
/// </summary>
public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IHttpWebClient _httpClient;
    private readonly ILogger<ExchangeRateProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateProvider"/> class.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance configured for CNB API access</param>
    /// <param name="logger">Logger instance for diagnostic information</param>
    public ExchangeRateProvider(IHttpWebClient httpClient, ILogger<ExchangeRateProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves exchange rates for specified currencies from CNB.
    /// </summary>
    /// <param name="currencies">Collection of currencies to fetch rates for</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a collection of <see cref="ExchangeRate"/> objects for currencies that exist in CNB's feed.
    /// </returns>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        _logger.LogInformation("Fetching exchange rates from CNB API");
        try
        {
            // Get data from httpClient
            var response = await _httpClient.GetAsync();
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Successfully received response from CNB API");

            // Read and parse data
            using var responseStream = await response.Content.ReadAsStreamAsync();

            return await ParseExchangeRatesAsync(responseStream, currencies);
        }
        catch (HttpRequestException ex) 
        {
            _logger.LogError(ex, "CNB API error");
            throw ;
        }
        catch (CsvHelperException ex)
        {
            _logger.LogError(ex, "Failed to parse CNB exchange rate data");
            throw ;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error in GetExchangeRatesAsync");
            throw ;
        }
    }

    private async Task<IEnumerable<ExchangeRate>> ParseExchangeRatesAsync(Stream responseStream, IEnumerable<Currency> currencies)
    {
        _logger.LogInformation("Parse received response data");

        using var reader = new StreamReader(responseStream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "|",
            HasHeaderRecord = false,
            MissingFieldFound = null,
            BadDataFound = context =>
            {
                // Log bad data if needed
                _logger.LogWarning("Bad data found at row {Parser}: {RawRecord}",
                    context.Context.Parser?.Row, context.RawRecord);
                Console.WriteLine($"Bad data found at row {context.Context.Parser?.Row}: {context.RawRecord}");
            }
        });

        // Skip the first two lines (date and headers)
        await reader.ReadLineAsync(); // Date line
        await reader.ReadLineAsync(); // Header line        

        // Create a set of currency codes to check against
        var currenciesSet = new HashSet<string>(currencies.Select(c => c.Code));

        //Parse data to list of CnbExchangeRate
        List<CnbExchangeRate> records = new List<CnbExchangeRate>();
        while (await csv.ReadAsync())
        {
            //skip invalid data
            if (csv.ColumnCount!=5 ||  csv.GetField<string>(3)?.Length != 3)
            {
                _logger.LogWarning("Skipping row {Row}: Invalid data",
                    csv.Context.Parser?.Row);
                continue;
            }
            records.Add( new CnbExchangeRate
            {
                // Manual validation for CNB's specific format
                Country = csv.GetField(0),
                CurrencyName = csv.GetField<string>(1),
                Amount = csv.GetField<decimal>(2),
                Code = csv.GetField<string>(3),
                Rate = csv.GetField<decimal>(4)
            });           
        }
        return records.Where(x => currenciesSet.Contains(x.Code)).Select(record =>
        {
            var amount = record.Amount;
            var rate = record.Rate;
            var normalizedRate = decimal.Round(rate / amount, 4);

            return new ExchangeRate(
                new Currency("CZK"),
                new Currency(record.Code),
                normalizedRate);

        }).ToList();
    }

}