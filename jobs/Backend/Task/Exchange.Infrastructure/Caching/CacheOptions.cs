namespace Exchange.Infrastructure.Caching;

public class CacheOptions
{
    public const string SectionName = "Cache";
    public TimeSpan DefaultAbsoluteExpiration { get; set; } = TimeSpan.FromMinutes(5);
}