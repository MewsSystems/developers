namespace Exchange.Infrastructure.Caching;

public class CacheOptions
{
    public TimeSpan DefaultAbsoluteExpiration { get; set; } = TimeSpan.FromMinutes(5);
}