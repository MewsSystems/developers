using System.Diagnostics;
using Mews.ExchangeRateUpdater.Application;
using Mews.ExchangeRateUpdater.Fetcher;
using Mews.ExchangeRateUpdater.Infrastructure;
using Mews.ExchangeRateUpdater.Infrastructure.Logging;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance;
using Serilog;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Activity.ForceDefaultIdFormat = true;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.With<TraceIdEnricher>() // Custom enricher to add TraceId
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {TraceId}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Fetcher service...");
    
    // Configure paths
    var isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    var dbPath = isRunningInDocker
        ? "/app/data/app.db"
        : Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ExchangeRateUpdater",
            "app.db"
        );

    Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        })
        .ConfigureServices((ctx, services) =>
        {
            var cnbUrl = ctx.Configuration["Cnb:BaseUrl"];
            if (string.IsNullOrWhiteSpace(cnbUrl))
                throw new InvalidOperationException("Missing CNB base URL configuration (Cnb:BaseUrl).");

            var connectionString = $"Data Source={dbPath}";
            
            // DI
            services.AddInfrastructureServices(connectionString, cnbUrl);
            services.AddApplicationServices();

            services.AddHostedService<Worker>();
        })
        .Build();

    using (var scope = host.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    await host.RunAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
finally
{
    Log.CloseAndFlush();
}