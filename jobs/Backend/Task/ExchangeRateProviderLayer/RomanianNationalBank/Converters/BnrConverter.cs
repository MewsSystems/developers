using Common.DTOs;
using Common.Interfaces;
using RomanianNationalBank.Models;
using System.Globalization;

namespace RomanianNationalBank.Converters;

/// <summary>
/// Converts BNR (Romanian National Bank) XML response to a list of ExchangeRateDTO.
/// </summary>
public class BnrConverter : IExchangeRateConverter<BnrDataSet>
{
    private readonly string _baseCurrency;

    /// <summary>
    /// Initializes a new instance of the BnrConverter.
    /// </summary>
    /// <param name="baseCurrency">The base currency code (e.g., "RON")</param>
    /// <exception cref="ArgumentException">Thrown when baseCurrency is null or whitespace</exception>
    public BnrConverter(string baseCurrency)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency))
            throw new ArgumentException("Base currency cannot be null or empty", nameof(baseCurrency));

        _baseCurrency = baseCurrency.ToUpperInvariant();
    }

    /// <summary>
    /// Converts the BNR DataSet response to a standardized list of ExchangeRateDTO.
    /// </summary>
    /// <param name="response">The BNR XML response deserialized to BnrDataSet</param>
    /// <returns>List of exchange rates with configured base currency</returns>
    /// <exception cref="ArgumentNullException">When response is null</exception>
    /// <exception cref="InvalidOperationException">When response structure is invalid</exception>
    public Task<List<ExchangeRateDTO>> Convert(BnrDataSet response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response), "BNR response cannot be null");

        if (response.Body == null)
            throw new InvalidOperationException("BNR response body is missing");

        if (response.Body.Cubes == null || response.Body.Cubes.Count == 0)
            throw new InvalidOperationException("BNR response contains no exchange rate data");

        var exchangeRates = new List<ExchangeRateDTO>();

        // BNR typically has one Cube per day, but we'll process all available
        foreach (var cube in response.Body.Cubes)
        {
            // Skip null cubes
            if (cube == null)
                continue;

            if (cube.Rates == null || cube.Rates.Count == 0)
                continue;

            // Parse the cube date (format: yyyy-MM-dd)
            if (string.IsNullOrWhiteSpace(cube.Date))
                continue;

            if (!DateOnly.TryParse(cube.Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var validDate))
                continue;

            foreach (var rate in cube.Rates)
            {
                // Skip null rates
                if (rate == null)
                    continue;

                // Validate rate data
                if (string.IsNullOrWhiteSpace(rate.Currency))
                    continue;

                if (rate.Value <= 0)
                    continue;

                if (rate.Multiplier <= 0)
                    continue;

                // BNR rates are in format: 1 [Currency] = [Value] BaseCurrency
                // The rate represents: BaseCurrency → TargetCurrency

                exchangeRates.Add(new ExchangeRateDTO
                {
                    BaseCurrencyCode = _baseCurrency,
                    TargetCurrencyCode = rate.Currency.ToUpperInvariant(),
                    Multiplier = rate.Multiplier,
                    Rate = rate.Value,
                    ValidDate = validDate
                });
            }
        }

        if (exchangeRates.Count == 0)
            throw new InvalidOperationException("No valid exchange rates found in BNR response");

        return Task.FromResult(exchangeRates);
    }
}
