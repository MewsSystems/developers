namespace ExchangeRateUpdater.Domain.Entities;

public record Currency(string Code)
{
	/// <summary>
	/// Three-letter ISO 4217 code of the currency.
	/// </summary>
	public string Code { get; } = Code;

	public override string ToString()
	{
		return Code;
	}
}