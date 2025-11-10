using Common.DTOs;
using Common.Interfaces;
using CzechNationalBank.Models;
using System.Globalization;

namespace CzechNationalBank.Converters;

/// <summary>
/// Converts CNB (Czech National Bank) XML response to a list of ExchangeRateDTO.
/// </summary>
public class CnbConverter : IExchangeRateConverter<CnbExchangeRates>
{
    private readonly string _baseCurrency;

    /// <summary>
    /// Initializes a new instance of the CnbConverter.
    /// </summary>
    /// <param name="baseCurrency">The base currency code (e.g., "CZK")</param>
    /// <exception cref="ArgumentException">Thrown when baseCurrency is null or whitespace</exception>
    public CnbConverter(string baseCurrency)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency))
            throw new ArgumentException("Base currency cannot be null or empty", nameof(baseCurrency));

        _baseCurrency = baseCurrency.ToUpperInvariant();
    }

    /// <summary>
    /// Converts the CNB XML response to a standardized list of ExchangeRateDTO.
    /// </summary>
    /// <param name="response">The CNB XML response deserialized to CnbExchangeRates</param>
    /// <returns>List of exchange rates with configured base currency</returns>
    /// <exception cref="ArgumentNullException">When response is null</exception>
    /// <exception cref="InvalidOperationException">When response structure is invalid</exception>
    public Task<List<ExchangeRateDTO>> Convert(CnbExchangeRates response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response), "CNB response cannot be null");

        if (response.Table == null)
            throw new InvalidOperationException("CNB response table is missing");

        if (response.Table.Rates == null || response.Table.Rates.Count == 0)
            throw new InvalidOperationException("CNB response contains no exchange rate data");

        // Parse the response date (format: DD.MM.YYYY)
        if (string.IsNullOrWhiteSpace(response.Date))
            throw new InvalidOperationException("CNB response date is missing");

        if (!DateOnly.TryParseExact(response.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validDate))
            throw new InvalidOperationException($"CNB response date '{response.Date}' is not in expected format (DD.MM.YYYY)");

        var exchangeRates = new List<ExchangeRateDTO>();

        var czechCulture = new CultureInfo("cs-CZ");

        foreach (var rate in response.Table.Rates)
        {
            // Validate rate data
            if (string.IsNullOrWhiteSpace(rate.Code))
                continue;

            if (string.IsNullOrWhiteSpace(rate.RateValue))
                continue;

            if (rate.Amount <= 0)
                continue;

            // Parse using Czech culture (comma as decimal separator)
            if (!decimal.TryParse(rate.RateValue?.Trim(), NumberStyles.Number, czechCulture, out var rateValue))
            {
                // They changed the style?
                if (!decimal.TryParse(rate.RateValue?.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out rateValue))
                {
                    continue;
                }
            }

            if (rateValue <= 0)
                continue;

            // CNB rates are in format: [Amount] [Currency] = [RateValue] CZK
            // Example: 1 USD = 22.456 CZK (means 1 USD costs 22.456 CZK)
            // The rate represents: BaseCurrency (CZK) → TargetCurrency (USD)
            // This matches the BNR pattern: local currency → foreign currency
            exchangeRates.Add(new ExchangeRateDTO
            {
                BaseCurrencyCode = _baseCurrency,
                TargetCurrencyCode = rate.Code.ToUpperInvariant(),
                Multiplier = rate.Amount,
                Rate = rateValue,
                ValidDate = validDate
            });
        }

        if (exchangeRates.Count == 0)
            throw new InvalidOperationException("No valid exchange rates found in CNB response");

        return Task.FromResult(exchangeRates);
    }
}
