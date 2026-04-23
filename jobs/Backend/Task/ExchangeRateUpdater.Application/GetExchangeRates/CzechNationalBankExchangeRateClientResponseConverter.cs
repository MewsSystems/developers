using System.Globalization;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class CzechNationalBankExchangeRateClientResponseConverter : ICzechNationalBankExchangeRateClientResponseConverter
{
    public List<ExchangeRate> Convert(string? clientResponse)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clientResponse);
        
        var responseLines = clientResponse.Split('\n');
        var exchangeRateLines = responseLines.Skip(2).Where(line => !string.IsNullOrEmpty(line));
        return exchangeRateLines.Select(ConvertLine).ToList();
    }

    private static ExchangeRate ConvertLine(string line)
    {
        const char lineSeparator = '|';
        var lineParts = line.Split(lineSeparator).ToList();
        ValidateLineFormat(lineParts);

        var amount = GetAmount(lineParts);
        var rate = GetRate(lineParts);
        var rateByUnit = decimal.Divide(rate, amount);

        var sourceCurrencyCode = lineParts[3];
        var sourceCurrency = GetCurrency(sourceCurrencyCode);
        const string targetCurrencyCode = "CZK";
        var targetCurrency = GetCurrency(targetCurrencyCode);
        return new ExchangeRate(sourceCurrency, targetCurrency, rateByUnit);
    }

    private static void ValidateLineFormat(List<string> lineParts)
    {
        if (lineParts.Count != 5) throw new InvalidExchangeRateLineFormatException();
    }

    private static int GetAmount(List<string> lineParts)
    {
        const int amountColumnIndex = 2;
        var value = lineParts[amountColumnIndex];
        if (!int.TryParse(value, out var amount))
        {
            throw new InvalidAmountFormatException(value);
        }

        return amount;
    }
    
    private static decimal GetRate(List<string> lineParts)
    {
        const int rateColumnIndex = 4;
        var value = lineParts[rateColumnIndex];
        if (!decimal.TryParse(value, CultureInfo.InvariantCulture, out var rate))
        {
            throw new InvalidRateFormatException(value);
        }

        return rate;
    }

    private static Currency GetCurrency(string code)
    {
        try
        {
            var currency = new Currency(code);
            return currency;
        }
        catch (ArgumentException argumentException) when (argumentException.ParamName == nameof(Currency.Code))
        {
            throw new InvalidCurrencyCodeException(code, argumentException);
        }
    }
}