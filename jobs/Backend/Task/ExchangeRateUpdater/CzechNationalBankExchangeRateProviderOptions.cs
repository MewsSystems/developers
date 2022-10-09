using System;

namespace ExchangeRateUpdater;

public class CzechNationalBankExchangeRateProviderOptions
{
    public Uri BaseAddress { get; set; }
    public Uri ExchangeRatesEndpoint { get; set; }
}
