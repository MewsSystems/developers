using System.Globalization;

namespace ExchangeRates.Providers
{
	public interface ICultureProvider
	{
		CultureInfo GetCultureInfo();
	}
}
