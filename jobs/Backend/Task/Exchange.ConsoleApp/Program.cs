using Exchange.Application.Extensions;
using Exchange.ConsoleApp;
using Exchange.Infrastructure.Extensions.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddConsole();
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDateTimeProvider();
        services.AddCnbApiClient(context.Configuration);
        services.AddInMemoryCache(context.Configuration);
        services.AddExchangeRateProvider();
        services.AddTransient<App>();
    })
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = true;
        options.ValidateOnBuild = true;
    })
    .Build();

using var scope = host.Services.CreateScope();
await scope.ServiceProvider
    .GetRequiredService<App>()
    .RunAsync();