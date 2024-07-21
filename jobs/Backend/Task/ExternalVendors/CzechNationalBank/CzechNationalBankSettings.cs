using System;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank;

public class CzechNationalBankSettings
{
    public const string Position = "CNB_API";
    public Uri BASE_URL { get; init; }

    /// <summary>
    /// Represents the refresh rate (in minutes) for retrieving exchange rates from the external vendor.
    /// </summary>
    public int REFRESH_RATE_IN_MINUTES { get; init; }

    /// <summary>
    /// Represents the key used to store exchange rates in the rate storage.
    /// </summary>
    public string RATE_STORAGE_KEY { get; init; }
}