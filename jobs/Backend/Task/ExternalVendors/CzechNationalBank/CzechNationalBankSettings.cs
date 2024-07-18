using System;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank;

public class CzechNationalBankSettings
{
    public const string Position = "CNB_API";
    public Uri BASE_URL { get; set; } = null;
}