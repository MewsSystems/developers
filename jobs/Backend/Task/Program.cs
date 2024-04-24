namespace ExchangeRateUpdater;


public static class Program
{
    private static IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    public static async Task Main( string[] args )
    {
        try
        {
            var env = Environment.GetEnvironmentVariable( ApplicationConstants.EnvironmentParam );
            Log( "Starting up..." );
            Log( $"Environment is {env}" );

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath( Directory.GetCurrentDirectory() )
                .AddJsonFile( $"{ApplicationConstants.ApplicationSettingsName}.json", false, true )
                .AddJsonFile( $"{ApplicationConstants.ApplicationSettingsName}.{env}.json", true, false )
                .Build();

            var cnbApiBaseUrl = configurationRoot.GetSection( "CNBApiUrl" ).Value;
            var cnbApiRequestTimeoutInSeconds = int.Parse( configurationRoot.GetSection( "CNBApiRequestTimeoutInSeconds" ).Value );
            var cnbApiRequestDateFormat = configurationRoot.GetSection( "CNBApiRequestDateFormat" ).Value;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient<IApiHttpClient, CNBApiHttpClient>( c =>
            {
                c.BaseAddress = new Uri( $"{cnbApiBaseUrl}{DateTime.Today.ToString( cnbApiRequestDateFormat )}" );
                c.Timeout = TimeSpan.FromSeconds( cnbApiRequestTimeoutInSeconds );
                c.DefaultRequestHeaders.Clear();
            } );

            serviceCollection.Configure<FaultHandlingSettings>( configurationRoot.GetSection( "FaultHandling" ) );

            var serviceProvider = serviceCollection
                .AddLogging( logging =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration( configurationRoot );
                    logging.AddConsole();
                } )
                .AddScoped<IExchangeRateProviderService, CNBExchangeRateProviderService>()
                .AddScoped<IExchangeRateProviderRepository, CNBExchangeRateProviderRepository>()
                .AddSingleton<IHttpRetryPolicy, HttpRetryPolicy>()
                .AddSingleton<IConfiguration>( configurationRoot )
                .BuildServiceProvider();

            var exchangeRateProvider = serviceProvider.GetService<IExchangeRateProviderService>();
            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync( currencies );

            Log( $"\nSuccessfully retrieved {exchangeRates.Count()} exchange rates:" );

            foreach( var rate in exchangeRates )
            {
                Log( rate.ToString() );
            }
        }
        catch( Exception e )
        {
            Log( $"Could not retrieve exchange rates: '{e.Message}'." );
        }

        Console.ReadLine();
    }

    private static void Log( string message ) => Console.WriteLine( message );
}