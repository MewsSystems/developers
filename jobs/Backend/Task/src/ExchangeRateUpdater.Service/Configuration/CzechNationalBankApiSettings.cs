using Domain.Ports;

namespace ExchangeRatesSearcherService.Configuration;

public class CzechNationalBankApiSettings : IExchangeRateApiSettings
{
    public string ApiBaseAddress { get; }
    public string Delimiter { get; }
    public string DecimalSeparator { get; }
    
    public CzechNationalBankApiSettings(string apiBaseUrl, string delimiter, string decimalSeparator)
    {
        if (string.IsNullOrWhiteSpace(apiBaseUrl))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiBaseUrl));
        if (string.IsNullOrWhiteSpace(delimiter))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(delimiter));
        if (string.IsNullOrWhiteSpace(decimalSeparator))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(decimalSeparator));
        
        ApiBaseAddress = apiBaseUrl;
        Delimiter = delimiter;
        DecimalSeparator = decimalSeparator;
    }
}