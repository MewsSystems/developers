using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Adapters;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Providers;

internal class ExchangeRateProvider : IExchangeRateProvider
{
	private readonly IExchangeRateApiAdapter _exchangeRateApi;

	public ExchangeRateProvider(IExchangeRateApiAdapter exchangeRateApi)
	{
		_exchangeRateApi = exchangeRateApi;
	}
	
	public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies,
		CancellationToken cancellationToken)
	{
		var currenciesArray = currencies as Currency[] ?? currencies.ToArray();
		if (!currenciesArray.Any())
		{
			throw new ArgumentException($"{nameof(currencies)} can not be empty");
		}

		var exchangeRates = await _exchangeRateApi.GetExchangesRateAsync(cancellationToken);

		return exchangeRates.Where(x => currenciesArray.Contains(x.SourceCurrency, new CurrencyComparer()) || currenciesArray.Contains(x.TargetCurrency, new CurrencyComparer()));
	}
		
}