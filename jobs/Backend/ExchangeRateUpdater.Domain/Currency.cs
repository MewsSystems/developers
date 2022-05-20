using ExchangeRateUpdater.Domain;

using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public record Currency
    {
        private static readonly Regex IsValidCurrencyCode = new Regex(@"(^[A-Z]{3}$)", RegexOptions.Compiled);

        public string Value { get; }

        private Currency(string value) => Value = value;

        public static Result<Currency> Create(string? currencyOrNothing)
            => currencyOrNothing
                .ToResult("Currency Code is null")
                .OnSuccess(currency => currency.ToUpperInvariant())
                .Ensure(currency => currency != string.Empty, "Cuurency Code is empty")
                .Ensure(IsValidCurrencyCode.IsMatch, "Curency code not in the correct format")
                .Map(currencyCode => new Currency(currencyCode));

        public static explicit operator Currency(string currency) => Create(currency).Value;
        public static implicit operator string(Currency currency) => currency.Value;
    }
}
