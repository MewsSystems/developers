using ApiClients.CzechNationalBank.Models;
using Core.Services.CzechNationalBank;
using Core.Services.CzechNationalBank.Interfaces;
using Data;
using Data.Database;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Infrastructure.CzechNationalBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Serilog;
using Services.Handlers.CzechNationalBank;
using System.Reflection;

namespace Tests;

[TestFixture]
public abstract class ProductionTestFixture
{
    protected ILogger _logger;
    protected ServiceProvider _serviceProvider;
    protected ApplicationDbContext ActContext;
    protected ApplicationDbContext ArrangeContext;
    protected ApplicationDbContext AssertContext;
    protected Mock<IHttpClientFactory> _httpClientFactoryMock;
    protected Mock<ICzechNationalBankHttpApiClient> _czechNationalBankHttpApiClientMock;
    protected CurrencyCzechRateBuilder _currencyCzechRateBuilder;
    protected readonly Mock<ICheckBankRateService> _checkBankRateServiceMock = new();

    [OneTimeSetUp]
    public async Task Setup()
    {
        _czechNationalBankHttpApiClientMock = new Mock<ICzechNationalBankHttpApiClient>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        SpecificSetup();

        var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.tests.json", optional: false, reloadOnChange: true);
        var configuration = configBuilder.Build();

        var services = ConfigureServices(configuration);

        ConfigureDbContexts(services, configuration);        

        _currencyCzechRateBuilder = new CurrencyCzechRateBuilder(ArrangeContext);
    }

    private IServiceCollection ConfigureDbContexts(IServiceCollection services, IConfigurationRoot? configuration)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("ConnectionDatabase"));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);       

        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("ConnectionDatabase"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        ArrangeContext = new ApplicationDbContext(optionsBuilder.Options);
        AssertContext = new ApplicationDbContext(optionsBuilder.Options);

        _serviceProvider = services.BuildServiceProvider();

        ActContext = _serviceProvider.GetService<ApplicationDbContext>();
        ActContext.Database.EnsureDeleted();
        ActContext.Database.Migrate();

        return services;
    }

    private IServiceCollection ConfigureServices(IConfigurationRoot configuration)
    {
        // SetUp DI
        var services = new ServiceCollection().AddLogging(builder => builder.AddSerilog(dispose: true));
        services.AddOptions();

        services.AddSingleton(_czechNationalBankHttpApiClientMock.Object);
        services.AddScoped(typeof(ILog<>), typeof(Log<>));
        services.AddScoped<IGenericRepository<CurrencyCzechRate>, GenericRepository<CurrencyCzechRate>>();
        services.AddScoped(provider => _checkBankRateServiceMock.Object);
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        services.Configure<CzechNationalBankApiOptions>(configuration.GetSection("CzechNationalBankApi").Bind);

        _logger = new LoggerConfiguration().CreateLogger();
        services.AddSingleton(_logger);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetRateRequestHandler).GetTypeInfo().Assembly));

        return services;
    }

    protected virtual void SpecificSetup()
    {
        var client = new HttpClient();
        _httpClientFactoryMock
            .Setup(x => x.CreateClient("CzechNationalBankApi"))
            .Returns(client);
    }

    private void CleanupRateTable()
    {
        foreach (var rate in ArrangeContext.CurrencyCzechRate)
        {
            ArrangeContext.CurrencyCzechRate.Remove(rate);
        }
        ArrangeContext.SaveChanges();
    }

    [TearDown]
    protected void TearDownGeneral()
    {
        ArrangeContext.ChangeTracker.Clear();
        ArrangeContext.Database.BeginTransaction();

        CleanupRateTable();
        _httpClientFactoryMock.Invocations.Clear();
        ArrangeContext.Database.CommitTransaction();
        ActContext.ChangeTracker.Clear();
        AssertContext.ChangeTracker.Clear();
        _czechNationalBankHttpApiClientMock.Invocations.Clear();
    }
}
