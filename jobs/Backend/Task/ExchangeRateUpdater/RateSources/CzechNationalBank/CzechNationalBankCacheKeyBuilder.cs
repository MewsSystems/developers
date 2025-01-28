using System;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public static class CzechNationalBankCacheKeyBuilder
{
    private const string VERSION = "v1";
    private const string PRIMARY = "CNB_PRIMARY";
    private const string SECONDARY = "CNB_SECONDARY";

    private const string DateFormat = "yyyy.MM.dd";
    

    public static string PrimaryCacheKey(DateOnly targetDate) => $"{VERSION}_{PRIMARY}_{targetDate.ToString(DateFormat)}";

    public static string SecondaryCacheKey(DateOnly targetDate) => $"{VERSION}_{SECONDARY}_{targetDate.ToString(DateFormat)}";
}