using ExchangeEntities;

namespace ExchangeRateTool.Interfaces
{
	public interface IExchangeRateFactory
	{
		ExchangeRate Build(string sourceCode, string targetCode, decimal value);
	}
}

