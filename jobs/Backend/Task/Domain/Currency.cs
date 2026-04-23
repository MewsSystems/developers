using System;

namespace ExchangeRateUpdater.Domain;

public sealed class Currency
{
    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be null, empty, or whitespace.", nameof(code));
        }

        Code = code.Trim().ToUpperInvariant();
    }

    public string Code { get; }

    public override string ToString()
    {
        return Code;
    }
}
