using Domain.Ports;

namespace ExchangeRateUpdater.Service.Tests.Unit.Configurations;

public class CzechNationalBankApiSettings : IExchangeRateApiSettings
{
    public string ApiBaseAddress { get; set; }
    public string Delimiter { get; set; }
    public string DecimalSeparator { get; set; }
}