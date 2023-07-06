using ExchangeEntities;
using ExchangeRateTool.Interfaces;

namespace ExchangeRateTool.Factories
{
	public class ExchangeRateFactory : IExchangeRateFactory
	{
        public ExchangeRate Build(string sourceCode, string targetCode, decimal value)
        {
            var sourceCurrency = new Currency(sourceCode);
            var targetCurrency = new Currency(targetCode);

            return new ExchangeRate(sourceCurrency, targetCurrency, value);
        }
    }
}

