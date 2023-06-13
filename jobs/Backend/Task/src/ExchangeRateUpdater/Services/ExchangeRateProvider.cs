﻿using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.Services
{
	/// <summary>
	/// Provides daily FX rates.
	/// </summary>
	/// <remarks>
	/// This is the original (slightly adjusted) class which adapts the newly created <see cref="ICnbFxRateProvider"/>.
	/// </remarks>
	public class ExchangeRateProvider : IExchangeRateProvider
	{
		private readonly ICnbFxRateProvider cnbFxRateProvider;

		public ExchangeRateProvider(ICnbFxRateProvider cnbFxRateProvider)
		{
			this.cnbFxRateProvider = cnbFxRateProvider;
		}

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
		{
			return await cnbFxRateProvider.GetExchangeRatesAsync(currencies, null);
		}
	}
}