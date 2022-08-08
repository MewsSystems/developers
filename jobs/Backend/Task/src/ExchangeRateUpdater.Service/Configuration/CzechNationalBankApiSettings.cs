using Domain.Ports;

namespace ExchangeRatesSearcherService.Configuration;

public class CzechNationalBankApiSettings : IExchangeRateApiSettings
{
    public string ApiBaseAddress { get; }
    public string DefaultExchangeRateTargetCurrency { get; }
    public string Delimiter { get; }
    public string DecimalSeparator { get; }
    
    public CzechNationalBankApiSettings(string apiBaseUrl, string defaultCurrency, string delimiter, string decimalSeparator)
    {
        if (string.IsNullOrWhiteSpace(apiBaseUrl))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiBaseUrl));
        if (string.IsNullOrWhiteSpace(defaultCurrency))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(defaultCurrency));
        if (string.IsNullOrWhiteSpace(delimiter))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(delimiter));
        if (string.IsNullOrWhiteSpace(decimalSeparator))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(decimalSeparator));
        
        ApiBaseAddress = apiBaseUrl;
        DefaultExchangeRateTargetCurrency = defaultCurrency;
        Delimiter = delimiter;
        DecimalSeparator = decimalSeparator;
    }
}