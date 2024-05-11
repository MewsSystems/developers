using ExchangeRateUpdater;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddOptions<ExternalBankApiSettings>()
            .Bind(context.Configuration.GetSection(ExternalBankApiSettings.ConfigurationName))
            .ValidateDataAnnotations();

        services.AddOptions<ApplicationSettings>()
            .Bind(context.Configuration.GetSection(ApplicationSettings.ConfigurationName));

        services.AddSingleton<IExternalBankApiClient, ExternalBankApiClient>();
        services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();