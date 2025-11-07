using ApplicationLayer.DTOs.Currencies;
using REST.Response.Models.Areas.Currencies;

namespace REST.Response.Converters;

/// <summary>
/// Converters for transforming Currency DTOs to API response models.
/// </summary>
public static class CurrencyConverters
{
    /// <summary>
    /// Converts CurrencyDto to CurrencyResponse.
    /// </summary>
    public static CurrencyResponse ToResponse(this CurrencyDto dto)
    {
        return new CurrencyResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = GetCurrencyName(dto.Code),
            Symbol = GetCurrencySymbol(dto.Code)
        };
    }

    /// <summary>
    /// Gets a human-readable currency name from the currency code.
    /// This is a simple implementation - in production, you might use a currency lookup service.
    /// </summary>
    private static string? GetCurrencyName(string code)
    {
        return code switch
        {
            "USD" => "United States Dollar",
            "EUR" => "Euro",
            "GBP" => "British Pound Sterling",
            "JPY" => "Japanese Yen",
            "CHF" => "Swiss Franc",
            "CAD" => "Canadian Dollar",
            "AUD" => "Australian Dollar",
            "CNY" => "Chinese Yuan",
            "CZK" => "Czech Koruna",
            "RON" => "Romanian Leu",
            _ => null
        };
    }

    /// <summary>
    /// Gets the currency symbol from the currency code.
    /// This is a simple implementation - in production, you might use a currency lookup service.
    /// </summary>
    private static string? GetCurrencySymbol(string code)
    {
        return code switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            "JPY" => "¥",
            "CHF" => "Fr",
            "CAD" => "$",
            "AUD" => "$",
            "CNY" => "¥",
            "CZK" => "Kč",
            "RON" => "lei",
            _ => null
        };
    }

    /// <summary>
    /// Converts CurrencyPairDto to CurrencyPairResponse.
    /// </summary>
    public static CurrencyPairResponse ToResponse(this CurrencyPairDto dto)
    {
        return new CurrencyPairResponse
        {
            BaseCurrency = dto.BaseCurrencyCode,
            TargetCurrency = dto.TargetCurrencyCode,
            ProviderCount = dto.ProviderCount,
            AvailableProviders = dto.AvailableProviders,
            LatestRateDate = dto.LatestRateDate?.ToString("yyyy-MM-dd"),
            LatestRate = dto.LatestRate
        };
    }
}
