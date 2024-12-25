using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Adapters;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Core.Providers;

internal class ExchangeRateProvider : IExchangeRateProvider
{
	private readonly IExchangeRateApiAdapter _exchangeRateApi;
	private readonly ILogger<ExchangeRateProvider> _logger;
	public ExchangeRateProvider(IExchangeRateApiAdapter exchangeRateApi, ILogger<ExchangeRateProvider> logger)
	{
		_exchangeRateApi = exchangeRateApi;
		_logger = logger;
	}
	
	public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies,
		CancellationToken cancellationToken)
	{
		var currenciesArray = currencies as Currency[] ?? currencies.ToArray();
		if (!currenciesArray.Any())
		{
			throw new ArgumentException($"{nameof(currencies)} can not be empty");
		}

		_logger.LogTrace("Start getting exchanges rates");
		var exchangeRates = await _exchangeRateApi.GetExchangesRateAsync(cancellationToken);
		_logger.LogTrace("Finish getting exchanges rates");
		
		return exchangeRates.Where(x => currenciesArray.Contains(x.SourceCurrency, new CurrencyComparer()) && currenciesArray.Contains(x.TargetCurrency, new CurrencyComparer()));
	}
		
}