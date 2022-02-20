using Common.Model;

namespace Common.Interface;

public interface IExchangeRateProvider
{
	/// <summary>
	/// Returns exchange rates among the specified currencies that are defined by the provider.
	/// </summary>
	/// <param name="currencies">Collection of requested currencies.</param>
	/// <returns>Exchange rates.</returns>
	Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);

	/// <summary>
	/// Returns exchange rates among the specified currencies that are defined by the provider.
	/// </summary>
	/// <param name="currencies">Collection of requested currencies.</param>
	/// <param name="dateTime">Query date.</param>
	/// <returns>Exchange rates.</returns>
	Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime dateTime);
}