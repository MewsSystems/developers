using System.Globalization;
using Domain.Entities;
using ExchangeRatesSearcherService.Configuration;
using Serilog;

namespace ExchangeRatesSearcherService.Mapper;

internal class ExchangeRatesMapper
{
    private readonly IReadOnlyList<string> _lines;
    private readonly ILogger _logger;
    private readonly CzechNationalBankApiSettings _settings;
    
    private const string AmountColumn = "Amount";
    private const string CurrencyColumn = "Code";
    private const string RateColumn = "Rate";

    private const string DefaultCurrency = "CZK";

    public ExchangeRatesMapper(IReadOnlyList<string> lines, ILogger logger, CzechNationalBankApiSettings settings)
    {
        _lines = lines ?? throw new ArgumentNullException(nameof(lines));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public IEnumerable<ExchangeRate> Map()
    {
        var columnNamesLine = _lines[1];
        
        var columns = columnNamesLine.Split(_settings.Delimiter).ToList();
        
        var amountIndex   = columns.IndexOf(AmountColumn);
        var currencyIndex = columns.IndexOf(CurrencyColumn);
        var rateIndex     = columns.IndexOf(RateColumn);

        if (amountIndex == -1 || currencyIndex == -1 || rateIndex == -1)
        {
            throw new Exception("Incorrect column information format.");
        }
        
        // skip date and column lines
        var exchangeRateLines = _lines.Skip(2);
        
        var exchangeRates = new List<ExchangeRate>();
        
        foreach (var exchangeRateInfo in exchangeRateLines)
        {
            var values = exchangeRateInfo.Split(_settings.Delimiter);
            
            if (!TryGetElement(values, currencyIndex, out var sourceCurrencyCode) ||
                !TryGetElement(values, rateIndex, out var rateValue) ||
                !TryGetElement(values, amountIndex, out var amountValue))
            {
                _logger.Warning($"Invalid row skipped due to string parsing error. Exchange rate line information: {exchangeRateInfo}");
                continue;
            }
            
            if (!TryParseToDecimal(rateValue!, out var rate) || 
                !TryParseToDecimal(amountValue!, out var amount))
            {
                _logger.Warning($"Invalid row skipped due to decimal parsing error. Exchange rate line information: {exchangeRateInfo}.");
                continue;
            }
            
            var sourceCurrency = new Currency(sourceCurrencyCode);
            var targetCurrency = new Currency(DefaultCurrency);
            var exchangeRateValue = Math.Round(rate/amount, 3);

            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, exchangeRateValue));
            
        }
        
        return exchangeRates;
    }
    
    private bool TryGetElement(IReadOnlyList<string> exchangeRateInfo, int index, out string? value)
    {
        if (index < exchangeRateInfo.Count && !string.IsNullOrWhiteSpace(exchangeRateInfo[index]))
        {
            value = exchangeRateInfo[index];
            return true;
        }

        _logger.Warning($"Error getting string value at index: {index}");
        value = default;
        return false;
    }

    private bool TryParseToDecimal(string? stringValue, out decimal decimalValue)
    {
        var numberFormat = new NumberFormatInfo
        {
            CurrencyDecimalSeparator = _settings.DecimalSeparator
        };

        if (decimal.TryParse(stringValue, NumberStyles.Currency, numberFormat, out decimalValue))
        {
            return true;
        }

        _logger.Warning($"Error parsing decimal value: {stringValue}");
        return false;
    }
}