namespace ExchangeRateUpdater.Api.Domain.Common;

public static class ErrorCodes
{
    // Validation Errors
    public const string ValidationFailed = "Validation.Failed";
    public const string ValidationInvalidCurrencyCode = "Validation.InvalidCurrencyCode";
    public const string ValidationInvalidDate = "Validation.InvalidDate";
    public const string ValidationInvalidRate = "Validation.InvalidRate";
    public const string ValidationInvalidAmount = "Validation.InvalidAmount";
    public const string ValidationCurrencyCodeNullOrWhitespace = "Validation.CurrencyCodeNullOrWhitespace";
    public const string ValidationCurrencyCodeInvalidLength = "Validation.CurrencyCodeInvalidLength";
    public const string ValidationCurrencyCodeInvalidCharacters = "Validation.CurrencyCodeInvalidCharacters";
    
    // Exchange Rate Errors
    public const string ExchangeRateCurrencyNotFound = "ExchangeRate.CurrencyNotFound";
    public const string ExchangeRateSourceUnavailable = "ExchangeRate.SourceUnavailable";
    public const string ExchangeRateNoRatesAvailable = "ExchangeRate.NoRatesAvailable";
}
