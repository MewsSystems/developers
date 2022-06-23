using System.Globalization;
using ExchangeRateUpdater.Models.Entities;

namespace ExchangeRateUpdater.Service.Cnb.Mappers;

public class CnbServiceMapper
{
    private readonly string _defaultCurrency;
    private readonly string _lineDelimiter;
    private readonly NumberFormatInfo _numberFormat;
    private readonly bool _throwExceptionOnError;

    private const string Amount   = "Amount";
    private const string Currency = "Code";
    private const string Rate     = "Rate";

    public CnbServiceMapper(string defaultCurrency, string lineDelimiter, string decimalSeparator, bool throwExceptionOnError)
    {
        _defaultCurrency       = defaultCurrency;
        _lineDelimiter         = lineDelimiter;
        _throwExceptionOnError = throwExceptionOnError;
        _numberFormat          = new NumberFormatInfo { CurrencyDecimalSeparator = decimalSeparator };
    }

    internal List<ExchangeRate> Map(string columnInfo, List<string> lines)
    {
        var columnValues = columnInfo.Split(_lineDelimiter).ToList();

        var amountIndex   = columnValues.IndexOf(Amount);
        var currencyIndex = columnValues.IndexOf(Currency);
        var rateIndex     = columnValues.IndexOf(Rate);

        if (amountIndex == -1 || currencyIndex == -1 || rateIndex == -1)
            throw new Exception("Incorrect column information format.");
        
        var exchangeRates = new List<ExchangeRate>();
        foreach (var line in lines)
        {
            var values = line.Split(_lineDelimiter);

            if (!TryGetElement(values, currencyIndex, out var sourceCurrencyCode) ||
                !TryGetElement(values, rateIndex, out var rateValue) ||
                !TryGetElement(values, amountIndex, out var amountValue))
            {
                continue;
            }

            var sourceCurrency = new Currency(sourceCurrencyCode!);
            var targetCurrency = new Currency(_defaultCurrency);

            if (!TryParseToDecimal(rateValue!, out var rate) || 
                !TryParseToDecimal(amountValue!, out var amount))
            {
                continue;
            }

            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, Math.Round(rate/amount, 3)));
        }

        return exchangeRates;
    }

    private bool TryGetElement(IReadOnlyList<string> array, int index, out string? value)
    {
        if (index < array.Count && !string.IsNullOrWhiteSpace(array[index]))
        {
            value = array[index]; 
            return true;
        }

        if (_throwExceptionOnError)
            throw new Exception($"Mapping error. String value could not be set. Index {index}");

        value = null;
        return false;
    }

    private bool TryParseToDecimal(string? stringValue, out decimal value)
    {
        if (decimal.TryParse(stringValue, NumberStyles.Currency, _numberFormat, out value))
            return true;

        if (_throwExceptionOnError)
            throw new Exception($"Mapping error. Could not parse {stringValue} to decimal value.");

        return false;
    }
}