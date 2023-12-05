using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCases;
using Serilog;
using Adapter.ExchangeRateProvider.CzechNatBank;
using ExchangeRateUpdater.Host.WebApi.Configuration;
using ExchangeRateUpdater.Host.WebApi.Middleware;
using System.Reflection;

namespace ExchangeRateUpdater.Host.WebApi;

/// <summary>
/// Class the configures the application host builder.
/// </summary>
public class ApplicationHostBuilder
{
    /// <summary>
    /// Application Name.
    /// </summary>
    private const string ApplicationName = "ExchangeRateUpdater";
    /// <summary>
    /// Settings instance used to configure the solution/host.
    /// </summary>
    private readonly ISettings _settings;
    /// <summary>
    /// Logging instance used in host.
    /// </summary>
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Constructor for ApplicationHostBuilder.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger">Instance of Serilog.ILogger</param>
    /// <exception cref="ArgumentNullException">throws in case either settings or logger is null.</exception>
    public ApplicationHostBuilder(ISettings? settings, Serilog.ILogger? logger)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// This method configures the host.
    /// </summary>
    public IHostBuilder Configure()
    {
        var applicationBuilder = new HostBuilder().ConfigureWebHost(webBuilder =>
        {
            ConfigureServices(webBuilder);
        }).UseSerilog();

        return applicationBuilder;
    }

    /// <summary>
    /// This method configures the services of host.
    /// </summary>
    /// <remarks>Made it protected and virtual since it will be overriden in unit tests.</remarks>
    /// <param name="webBuilder">Instance of IHostBuilder</param>
    protected virtual void ConfigureServices(IWebHostBuilder webBuilder)
    {
        webBuilder
            .ConfigureServices(services =>
            {
                services.AddTransient<CorrelationMiddleware>();
                services.AddTransient<RequestMiddleWare>();
                RegisterUseCases(services);
                RegisterAdapters(services);
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(_ =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    _.IncludeXmlComments(xmlPath);
                });
                services.AddSerilog(_logger);
                services.AddMvcCore();
            });
        webBuilder.UseKestrel();
        webBuilder.Configure(applicationBuilder =>
        {
            applicationBuilder.UseMiddleware<CorrelationMiddleware>();
            if (_settings.EnableSwagger)
            {
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                applicationBuilder.UseSwagger();
                applicationBuilder.UseSwaggerUI(_ =>
                {
                    _.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate Update API");
                    _.RoutePrefix = string.Empty;
                });
            }
            applicationBuilder.UseExceptionHandler("/Error");
            applicationBuilder.UseMiddleware<RequestMiddleWare>();
            applicationBuilder.UseRouting();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseAuthorization();
            
            applicationBuilder.UseEndpoints(configuration =>
            {
                configuration.MapControllers();
            });
        });
    }

    /// <summary>
    /// This method registers adapters for ports.
    /// </summary>
    /// <remarks>Made it protected and virtual since it will be overriden in unit tests.</remarks>
    /// <param name="services">Instance of IServiceCollection</param>
    protected virtual void RegisterAdapters(IServiceCollection services)
    {
        services.AddHttpClient($"{ApplicationName}-http-client",
        client =>
        {
            // Set the base address of the named client.
            client.BaseAddress = new Uri(_settings.CzechNationalBankBaseAddress);
        });
        services.AddSingleton<IExchangeRateProviderRepository, CzechNationalBankRepository>();
    }

    /// <summary>
    /// This method registers use cases for Domain.
    /// </summary>
    /// <param name="services">Instance of IServiceCollection</param>
    private void RegisterUseCases(IServiceCollection services)
    {
        services.AddSingleton<ExchangeUseCase>();
    }


}
