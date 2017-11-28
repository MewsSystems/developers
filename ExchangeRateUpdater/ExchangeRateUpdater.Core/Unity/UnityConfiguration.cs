namespace ExchangeRateUpdater.Unity {
	using System.Net.Http;
	using Blockchain;
	using Blockchain.Http;
	using Blockchain.Http.Configuration;
	using CeskaNarodniBanka;
	using CeskaNarodniBanka.Http;
	using CeskaNarodniBanka.Http.Configuration;
	using ExchangeRateUpdater.Configuration;
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

			/// HttpClient registration
			container.RegisterType<HttpClient>(new InjectionConstructor());
			/// CurrencyValidator registration
			container.RegisterType<ICurrencyValidator, ISO4217CurrencyValidator>();
			/// Open Exchange Rate Provider registration
			
			container.RegisterType<IOpenExchangeRateHttpClientConfiguration, OpenExchangeRateHttpClientConfiguration>();
			container.RegisterType<IOpenExchangeRateClientOptions, OpenExchangeRateClientOptions>(new InjectionConstructor("af5fe838215e429580bb7ba71cc77b9d"));
			container.RegisterType<IOpenExchangeRateClient, OpenExchangeRateClient>();
			container.RegisterType<IExchangeRateProvider, OpenExchangeRateProvider>("Open Exchange Rates (USD)");
			/// Blockchain Provider registration
			container.RegisterType<IBlockchainHttpClientConfiguration, BlockchainHttpClientConfiguration>();
			container.RegisterType<IBlockchainExchangeRateClient, BlockchainExchangeRateClient>();
			container.RegisterType<IExchangeRateProvider, BlockchainExchangeRateProvider>("Blockchain (BTC)");
			/// ČNB Provider registration
			container.RegisterType<ICnbHttpClientConfiguration, CnbHttpClientConfiguration>();
			container.RegisterType<ICnbExchangeRateClient, CnbExchangeRateClient>();
			container.RegisterType<IExchangeRateProvider, CnbExchangeRateProvider>("Czech National Bank (CZK)");

			return container;
		}
	}
}
