await Host
	.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("appsettings.json").Build())
	.ConfigureServices((context, services) =>
	{
		var configuration = context.Configuration;

		services.AddTransient<App>();
		services.AddOptions<AppOptions>().Bind(configuration);
		services.UseCzechNationalBankProvider(options => configuration.GetSection(ExchangeRateUpdater.Providers.CzechNationalBank.Options.ConfigKey).Bind(options));
	})
	.Build()
	.Services
	.GetRequiredService<App>()
	.StartAsync(args);