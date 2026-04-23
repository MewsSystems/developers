namespace ExchangeRateUpdater.Api.Domain.Common;

public static class ExchangeRateErrors
{
    public static Error CurrencyNotFound(string currencyCode) =>
        new(ErrorCodes.ExchangeRateCurrencyNotFound,
            $"Exchange rate for currency '{currencyCode}' was not found in the source data.",
            ErrorType.NotFound);

    public static readonly Error SourceUnavailable =
        new(ErrorCodes.ExchangeRateSourceUnavailable,
            "The exchange rate data source is currently unavailable. Please try again later.",
            ErrorType.Unavailable);

    public static Error InvalidCurrencyCode(string code) =>
        new(ErrorCodes.ValidationInvalidCurrencyCode,
            $"'{code}' is not a valid ISO 4217 currency code. Currency codes must be exactly 3 letters.",
            ErrorType.Validation);

    public static Error InvalidDate(string date) =>
        new(ErrorCodes.ValidationInvalidDate,
            $"'{date}' is not a valid date. Expected format: yyyy-MM-dd.",
            ErrorType.Validation);

    public static Error InvalidRate(decimal rate) =>
        new(ErrorCodes.ValidationInvalidRate,
            $"Exchange rate '{rate}' is invalid. Rate must be non-negative.",
            ErrorType.Validation);

    public static Error InvalidAmount(int amount) =>
        new(ErrorCodes.ValidationInvalidAmount,
            $"Amount '{amount}' is invalid. Amount must be a positive integer.",
            ErrorType.Validation);

    public static readonly Error NoRatesAvailable =
        new(ErrorCodes.ExchangeRateNoRatesAvailable,
            "No exchange rates are available for the specified criteria.",
            ErrorType.NotFound);
}
