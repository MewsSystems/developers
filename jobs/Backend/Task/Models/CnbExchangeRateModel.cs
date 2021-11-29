using System;

namespace ExchangeRateUpdater.Models;

/// <summary>
/// CNB exchange rate model taken from endpoint output
/// </summary>
public class CnbExchangeRateModel
{
    public string Country { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public Currency Code { get; set; }
    public decimal Rate { get; set; }

    public override bool Equals(object obj)
    {
        var model = obj as CnbExchangeRateModel;
        return obj is CnbExchangeRateModel &&
               model.Country == Country &&
               model.Currency == Currency &&
               model.Amount == Amount &&
               model.Code.Equals(Code) &&
               model.Rate == Rate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Country, Currency, Amount, Code, Rate);
    }
}