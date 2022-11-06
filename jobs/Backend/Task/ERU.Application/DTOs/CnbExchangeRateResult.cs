namespace ERU.Application.DTOs;

public record CnbExchangeRateResult
(
	string? Country,
	string? Currency,
	decimal? Amount,
	string? Code,
	decimal? Rate
)
{
	public CnbExchangeRateResult(decimal amount, string code, decimal rate ) : this(null, null, amount, code, rate)
	{
		Amount = amount;
		Code = code;
		Rate = rate;
	}
}