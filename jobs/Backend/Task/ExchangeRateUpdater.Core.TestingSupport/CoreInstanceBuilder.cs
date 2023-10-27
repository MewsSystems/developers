﻿using ExchangeRateUpdater.Core.Adapters;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;
using ExchangeRateUpdater.Core.Providers;

namespace ExchangeRateUpdater.Core.TestingSupport;

//This should be more elaborated, but the idea is to provide an easy way to get the instances for integration tests
public static class CoreInstanceBuilder
{
	public static IExchangeRateApiAdapter GetCzechNationalBankApiAdapter() => new CzechNationalBankApiAdapter(
		new HttpClient
		{
			BaseAddress = new Uri("https://api.cnb.cz/")
		});

	public static IExchangeRateProvider GetExchangeRateProvider(Func<IExchangeRateApiAdapter> exchangeRateApi) =>
		new ExchangeRateProvider(exchangeRateApi());
}