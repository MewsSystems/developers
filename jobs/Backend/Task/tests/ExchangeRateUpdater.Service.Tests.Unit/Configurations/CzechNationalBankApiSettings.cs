using Domain.Ports;

namespace ExchangeRateUpdater.Service.Tests.Unit.Configurations;

public class CzechNationalBankApiSettings : IExchangeRateApiSettings
{
    public string ApiBaseAddress { get; init; }
    public string Delimiter { get; init; }
    public string DecimalSeparator { get; init; }
}