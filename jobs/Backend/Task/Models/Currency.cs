namespace ExchangeRateUpdater.Models
{
    using System;

    public sealed class Currency : IEquatable<Currency>
    {
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Currency code is required", nameof(code));

            var normalized = code.Trim().ToUpperInvariant();
            if (normalized.Length != 3) throw new ArgumentException("Currency code must be 3 letters (ISO 4217)", nameof(code));

            Code = normalized;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public bool Equals(Currency? other) => other is not null && string.Equals(Code, other.Code, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) => Equals(obj as Currency);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Code);

        public override string ToString() => Code;
    }
}
