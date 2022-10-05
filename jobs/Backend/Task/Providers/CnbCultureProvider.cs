using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
