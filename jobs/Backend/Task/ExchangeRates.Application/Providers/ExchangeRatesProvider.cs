using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.External.CNB;

namespace ExchangeRates.Application.Providers
{
    public interface IExchangeRatesService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }

    public class ExchangeRatesProvider : IExchangeRatesService
    {
        private readonly CNBHttpClient _cnbHttpClient;

        public ExchangeRatesProvider(CNBHttpClient cnbHttpClient)
        {
            _cnbHttpClient = cnbHttpClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await _cnbHttpClient.GetDailyExchangeRatesAsync();

            if (response?.Rates == null || !response.Rates.Any())
                return Enumerable.Empty<ExchangeRate>();

            var result = new List<ExchangeRate>();
            var czk = new Currency("CZK");

            foreach (var target in currencies)
            {
                var rateEntry = response.Rates.FirstOrDefault(r => r.CurrencyCode == target.Code);
                if (rateEntry == null)
                    continue;

                var rateValue = rateEntry.Rate / rateEntry.Amount;

                result.Add(new ExchangeRate(czk, target, rateValue));
            }

            return result;
        }
    }
}
