using System;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank;

public class CzechNationalBankSettings
{
    public const string Position = "CNB_API";
    public Uri BASE_URL { get; init; }
    public int REFRESH_RATE_IN_MINUTES { get; init; }
    public string RATE_STORAGE_KEY { get; init; }
}