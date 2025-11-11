using Common.DTOs;
using Common.Interfaces;
using EuropeanCentralBank.Models;
using System.Globalization;

namespace EuropeanCentralBank.Converters;

/// <summary>
/// Converts ECB (European Central Bank) XML response to a list of ExchangeRateDTO.
/// Handles the triple-nested Cube structure: Envelope > Cube > Cube > Cube
/// </summary>
public class EcbConverter : IExchangeRateConverter<EcbEnvelope>
{
    private readonly string _baseCurrency;

    /// <summary>
    /// Initializes a new instance of the EcbConverter.
    /// </summary>
    /// <param name="baseCurrency">The base currency code (typically "EUR")</param>
    /// <exception cref="ArgumentException">Thrown when baseCurrency is null or whitespace</exception>
    public EcbConverter(string baseCurrency)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency))
            throw new ArgumentException("Base currency cannot be null or empty", nameof(baseCurrency));

        _baseCurrency = baseCurrency.ToUpperInvariant();
    }

    /// <summary>
    /// Converts the ECB Envelope response to a standardized list of ExchangeRateDTO.
    /// </summary>
    /// <param name="response">The ECB XML response deserialized to EcbEnvelope</param>
    /// <returns>List of exchange rates with configured base currency (EUR)</returns>
    /// <exception cref="ArgumentNullException">When response is null</exception>
    /// <exception cref="InvalidOperationException">When response structure is invalid</exception>
    public Task<List<ExchangeRateDTO>> Convert(EcbEnvelope response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response), "ECB response cannot be null");

        if (response.Cube == null)
            throw new InvalidOperationException("ECB response Cube element is missing");

        if (response.Cube.DateCubes == null || response.Cube.DateCubes.Count == 0)
            throw new InvalidOperationException("ECB response contains no date cube data");

        var exchangeRates = new List<ExchangeRateDTO>();

        // ECB daily file has one date cube, historical files may have multiple
        foreach (var dateCube in response.Cube.DateCubes)
        {
            // Skip null date cubes
            if (dateCube == null)
                continue;

            if (dateCube.Rates == null || dateCube.Rates.Count == 0)
                continue;

            // Parse the date cube time (format: YYYY-MM-DD)
            if (string.IsNullOrWhiteSpace(dateCube.Time))
                continue;

            if (!DateOnly.TryParse(dateCube.Time, CultureInfo.InvariantCulture, DateTimeStyles.None, out var validDate))
                continue;

            foreach (var rate in dateCube.Rates)
            {
                // Skip null rates
                if (rate == null)
                    continue;

                // Validate rate data
                if (string.IsNullOrWhiteSpace(rate.Currency))
                    continue;

                if (rate.Rate <= 0)
                    continue;

                // ECB rates are in format: 1 EUR = [rate] [currency]
                // Example: 1 EUR = 1.1492 USD
                // The rate represents: BaseCurrency (EUR) → TargetCurrency

                exchangeRates.Add(new ExchangeRateDTO
                {
                    BaseCurrencyCode = _baseCurrency,
                    TargetCurrencyCode = rate.Currency.ToUpperInvariant(),
                    Multiplier = 1, // ECB always uses 1 as multiplier
                    Rate = rate.Rate,
                    ValidDate = validDate
                });
            }
        }

        if (exchangeRates.Count == 0)
            throw new InvalidOperationException("No valid exchange rates found in ECB response");

        return Task.FromResult(exchangeRates);
    }
}
