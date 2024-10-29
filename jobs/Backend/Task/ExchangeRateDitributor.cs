using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
	public class ExchangeRateDitributor
	{
		private readonly Dictionary<string, ExchangeRate> _exchangeRates;
		public DateTime LastUpdateDate { get; }
		
		public ExchangeRateDitributor(IEnumerable<ExchangeRate> rates, DateTime lastUpdate)
		{
			_exchangeRates = rates.ToDictionary(r => r.SourceCurrency.Code);
			LastUpdateDate = lastUpdate;
		}
		
		public bool TryGet(Currency currency, out ExchangeRate exRate)
		{
			if(currency == null || string.IsNullOrEmpty(currency.Code))
			{
				exRate = null;
				return false;
			}
				
			return _exchangeRates.TryGetValue(currency.Code, out exRate);
		}
	}
}
