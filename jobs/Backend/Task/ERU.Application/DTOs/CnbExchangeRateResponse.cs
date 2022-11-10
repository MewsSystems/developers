namespace ERU.Application.DTOs;

public record CnbExchangeRateResponse
(
	string? Country,
	string? Currency,
	decimal? Amount,
	string? Code,
	decimal? Rate
)
{
	public CnbExchangeRateResponse(decimal amount, string code, decimal rate) : this(null, null, amount, code, rate)
	{
		Amount = amount;
		Code = code;
		Rate = rate;
	}
}