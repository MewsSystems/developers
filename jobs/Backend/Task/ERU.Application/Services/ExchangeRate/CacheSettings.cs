namespace ERU.Application.Services.ExchangeRate;

public record CacheSettings
{
	public int SlidingExpirationInMinutes { get; set; }
	public int AbsoluteExpirationInMinutes { get; set; }
}