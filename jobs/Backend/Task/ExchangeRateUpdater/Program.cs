using ExchangeRateUpdater;
using ExchangeRateUpdater.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ExchangeRatesConfig>(builder.Configuration.GetSection("ExchangeRates"));

builder.Services.AddTransient<ExchangeRateProvider>();

builder.Services.AddHttpClient();

builder.Services.AddHostedService<ExchangeRateWorker>();
    
var app = builder.Build();
app.Run();