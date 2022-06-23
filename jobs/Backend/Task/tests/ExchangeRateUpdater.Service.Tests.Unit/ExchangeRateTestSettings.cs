using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.Service.Tests.Unit;

public class ExchangeRateTestSettings : IExchangeRateServiceSettings
{
    public string BaseUrl { get; } = "https://localhost";
    public string TimezoneId { get; } = A.TimezoneId;
    public string DefaultCurrency { get; } = A.TestTargetCurrency.Code;
    public string MappingDelimiter { get; } = A.MappingDelimiter;
    public string MappingDecimalSeparator { get; } = A.MappingDecimalSeparator;
    public bool ThrowExceptionOnMappingErrors { get; } = true;
    public bool UseInMemoryCache { get; } = false;
    public TimeSpan CacheExpiryTime { get; } = new TimeSpan(00,00,00);
}