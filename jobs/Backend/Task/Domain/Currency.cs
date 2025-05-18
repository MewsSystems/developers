using System;

namespace ExchangeRateUpdater.Domain
{
    public record Currency
    {
        public string Code { get; }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));
            }

            Code = code.Trim().ToUpperInvariant();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
