using Exchange.Application.Extensions;
using Exchange.ConsoleApp;
using Exchange.Infrastructure.Extensions.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
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