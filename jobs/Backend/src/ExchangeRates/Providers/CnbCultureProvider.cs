using System.Globalization;

namespace ExchangeRates.Providers
{
	public class CnbCultureProvider : ICnbCultureProvider
	{
		private readonly CultureInfo culture;

		public CnbCultureProvider()
		{
			this.culture = CultureInfo.CreateSpecificCulture("cs-CZ");
		}

		public CultureInfo GetCultureInfo()
		{
			return this.culture;
		}
	}
}
