namespace Exchange.Domain.ValueObjects;

public record Currency
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; init; }

    public Currency(string Code) => this.Code = Code;

    public void Deconstruct(out string code) => code = this.Code;
}