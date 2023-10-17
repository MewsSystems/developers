using FileHelpers;

namespace Mews.ExchangeRateProvider.Mappers;

public sealed class CzechNationalBankExchangeRateMapper
{
    private const string CzechKorunaCurrencyCode = "CZK";

    private static readonly Currency DefaultCurrency = new(CzechKorunaCurrencyCode);

    internal IEnumerable<ExchangeRate> Read(string exchangeRateData)
    {
        var engine = new FileHelperEngine<CzechNationalBankExchangeRateRecord>();

        var textFileValues = engine.ReadString(exchangeRateData);

        foreach (var value in textFileValues)
        {
            yield return new ExchangeRate(new(value.Code), DefaultCurrency, value.Rate / value.Amount);
        }
    }
}