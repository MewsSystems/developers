using Microsoft.Extensions.Options;
using ExchangeRateUpdater.Api.Options;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Core.Clients;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for structured logging.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .ReadFrom.Configuration(builder.Configuration) // Reads logging settings from appsettings.json
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Rotating log files per day
    .CreateLogger();

builder.Host.UseSerilog();

// Bind CNB API options from appsettings.json
builder.Services.Configure<CnbOptions>(builder.Configuration.GetSection("CnbOptions"));

builder.Services.AddControllers();

// Enable API documentation via Swagger & annotations
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

// Register CNB client & provider
// - `CnbExchangeRateClient` makes HTTP requests to the CNB API.
// - `CnbExchangeRateProvider` processes the retrieved data.
builder.Services.AddHttpClient<ICnbExchangeRateClient, CnbExchangeRateClient>()
    .ConfigureHttpClient((sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<CnbOptions>>().Value;
        client.BaseAddress = new Uri(options.BaseUrl);
    });

builder.Services.AddScoped<IExchangeRateProvider, CnbExchangeRateProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.Run();
