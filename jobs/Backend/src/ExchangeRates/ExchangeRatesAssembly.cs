using System.Reflection;

namespace ExchangeRates
{
	public static class ExchangeRatesAssembly
	{
		public static Assembly Get()
		{
			return typeof(ExchangeRatesAssembly).Assembly;
		}
	}
}
