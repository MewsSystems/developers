using ExchangeEntities;

namespace ExchangeRateTool.Interfaces
{
	public interface IExchangeRateFactory
	{
		/// <summary>
		/// Build a <see cref="ExchangeRate"/> object with the desired parameters.
		/// </summary>
		/// <param name="sourceCode">Code for the source <see cref="Currency"/>.</param>
		/// <param name="targetCode">Code for the target <see cref="Currency"/>.</param>
		/// <param name="value">Rate value.</param>
		/// <returns><see cref="ExchangeRate"/> object.</returns>
		ExchangeRate Build(string sourceCode, string targetCode, decimal value);
	}
}

