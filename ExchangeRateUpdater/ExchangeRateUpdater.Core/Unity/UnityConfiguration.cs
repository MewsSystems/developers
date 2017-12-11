namespace ExchangeRateUpdater.Unity {
	using System.Collections.Generic;
	using System.Net.Http;
	using Blockchain;
	using Blockchain.Http;
	using Blockchain.Http.Configuration;
	using CeskaNarodniBanka;
	using CeskaNarodniBanka.Http;
	using CeskaNarodniBanka.Http.Configuration;
	using ExchangeRateUpdater.Configuration;
	using ExchangeRateUpdater.Core;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;
	using global::Unity;
	using global::Unity.Injection;
	using OpenExchangeRates;
	using OpenExchangeRates.Http;
	using OpenExchangeRates.Http.Configuration;

	public class UnityConfiguration : IConfiguration<IUnityContainer> {
		public IUnityContainer Configure(IUnityContainer target) {
			var container = Ensure.IsNotNull(target);

			/// Currency enumeration singleton registration
			container.RegisterInstance<IEnumerable<Currency>>(new[] {
				new Currency("USD"),
				new Currency("EUR"),
				new Currency("CZK"),
				new Currency("JPY"),
				new Currency("KES"),
				new Currency("RUB"),
				new Currency("THB"),
				new Currency("TRY"),
				new Currency("XYZ")
			});
			/// HttpClient registration
			container.RegisterType<HttpClient>(new InjectionConstructor());
			/// CurrencyValidator registration
			container.RegisterType<ICurrencyValidator, ISO4217CurrencyValidator>();
			/// Open Exchange Rate Provider registration
			container.RegisterInstance<AppId>(new AppId("af5fe838215e429580bb7ba71cc77b9d"));
			container.RegisterType<IOpenExchangeRateHttpClientConfiguration, OpenExchangeRateHttpClientConfiguration>();
			container.RegisterType<IOpenExchangeRateClientOptions, OpenExchangeRateClientOptions>();
			container.RegisterType<IOpenExchangeRateClient, OpenExchangeRateClient>();
			container.RegisterType<IOpenExchangeRateProviderOptions, OpenExchangeRateProviderOptions>();
			container.RegisterType<IExchangeRateProvider, OpenExchangeRateProvider>("Open Exchange Rates (USD)");
			/// Blockchain Provider registration
			container.RegisterType<IBlockchainHttpClientConfiguration, BlockchainHttpClientConfiguration>();
			container.RegisterType<IBlockchainExchangeRateClientOptions, BlockchainExchangeRateClientOptions>();
			container.RegisterType<IBlockchainExchangeRateClient, BlockchainExchangeRateClient>();
			container.RegisterType<IBlockchainExchangeRateProviderOptions, BlockchainExhangeRateProviderOptions>();
			container.RegisterType<IExchangeRateProvider, BlockchainExchangeRateProvider>("Blockchain (BTC)");
			/// ČNB Provider registration
			container.RegisterType<ICnbHttpClientConfiguration, CnbHttpClientConfiguration>();
			container.RegisterType<ICnbExchangeRateClientOptions, CnbExchangeRateClientOptions>();
			container.RegisterType<ICnbExchangeRateClient, CnbExchangeRateClient>();
			container.RegisterType<ICnbExchangeRateProviderOptions, CnbExchangeRateProviderOptions>();
			container.RegisterType<IExchangeRateProvider, CnbExchangeRateProvider>("Czech National Bank (CZK)");
			/// ProgramStep registration
			container.RegisterType<IProgramStep, SelectProviderStep>("0");
			container.RegisterType<IProgramStep, GetExchangeRatesStep>("1");
			container.RegisterType<IProgramStep, RestartProgramStep>("2");

			return container;
		}
	}
}
