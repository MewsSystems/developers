namespace ExchangeRateUpdater.Application.Common.Utils;

using ExchangeRateUpdater.Domain.Enums;

public static class CacheKeyHelper
{
    public static string RatesKey(Language language, DateTime date) =>
        $"exrates:{language.ToString().ToLower()}:{date:yyyy-MM-dd}";

    public static TimeSpan DefaultTtlYears(int years) => TimeSpan.FromDays(365 * Math.Max(1, years));
}