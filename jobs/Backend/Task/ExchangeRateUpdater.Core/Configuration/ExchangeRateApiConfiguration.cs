using ExchangeRateUpdater.Core.Interfaces;

namespace ExchangeRateUpdater.Core.Configuration;

public class ExchangeRateApiConfiguration : IApiConfiguration
{
    public string ApiUrl { get; set; } = null!;
}