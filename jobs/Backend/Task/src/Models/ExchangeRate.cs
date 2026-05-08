using System.Collections.Generic;
using System.Linq;
namespace ExchangeRateUpdater.Models;

public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }

    public static IEnumerable<ExchangeRateResponse> GetResponse(IEnumerable<ExchangeRate> rates)
    {
        return rates.Select(er => new ExchangeRateResponse
        {
            SourceCurrency = er.SourceCurrency.Code,
            TargetCurrency = er.TargetCurrency.Code,
            ExchangeRate = er.Value
        });
    }
}

