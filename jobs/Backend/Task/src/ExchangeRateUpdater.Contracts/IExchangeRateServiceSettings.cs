namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateServiceSettings
{
    string BaseUrl { get; }
    string TimezoneId { get; }
    string DefaultCurrency { get; }
    string MappingDelimiter { get; }
    string MappingDecimalSeparator { get; }
    bool ThrowExceptionOnMappingErrors { get; }
}