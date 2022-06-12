using System.Text;

namespace Mews.BackendDeveloperTask.ExchangeRates.Cnb;

public class CnbExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICnbTextExchangeRateRetriever _retriever;
    private readonly ICnbTextExchangeRateParser _parser;

    public CnbExchangeRateProvider(
        ICnbTextExchangeRateRetriever retriever,
        ICnbTextExchangeRateParser parser)
    {
        _retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var selectedCurrencies = currencies.ToHashSet();
        var text = await _retriever.GetDailyRatesAsync();
        var rates = _parser.Parse(text);
        return rates.Where(rate => selectedCurrencies.Contains(rate.Source));
    }
}
