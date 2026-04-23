namespace ExchangeRateUpdater.Api.Domain.Common;

public static class CurrencyErrors
{
    public static Error NullOrWhitespace() =>
        new(ErrorCodes.ValidationCurrencyCodeNullOrWhitespace, 
            "Currency code is missing.");

    public static Error InvalidLength(string code) =>
        new(ErrorCodes.ValidationCurrencyCodeInvalidLength,
            $"Currency code must be exactly 3 characters. Got '{code}' with length {code.Length}.");

    public static Error InvalidCharacters(string code) =>
        new(ErrorCodes.ValidationCurrencyCodeInvalidCharacters,
            $"Currency code must contain only letters. Got '{code}'.");
}
