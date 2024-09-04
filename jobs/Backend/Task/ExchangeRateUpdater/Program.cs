using ExchangeRateUpdater;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ExchangeRatesConfig>(builder.Configuration.GetSection("ExchangeRates"));

builder.Services.AddScoped<ExchangeRateProvider>();

builder.Services.AddHttpClient<ExchangeRateProvider>();

builder.Services.AddHostedService<ExchangeRateWorker>();
    
var app = builder.Build();
app.Run();