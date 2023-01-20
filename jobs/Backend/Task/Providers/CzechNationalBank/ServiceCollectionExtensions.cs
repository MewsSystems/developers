namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
    internal static class ServiceCollectionExtensions
	{
		public static IServiceCollection UseCzechNationalBankProvider(this IServiceCollection services, Action<Options> configureOptions)
		{
			services.Configure(configureOptions);
			services.AddHttpClient(Options.ConfigKey);
			services.AddTransient<IExchangeRateProvider, Provider>();
			return services;
		}
	}
}
