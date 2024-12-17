using ExchangeRateUpdater.Core.Models.Configuration;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Core.Services.Abstractions;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Build configuration, include appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Register services for dependency injection
builder.Services
    .AddSingleton<IExchangeRateProvider, CnbExchangeRateProvider>()
    .Configure<ExchangeRateSettings>(builder.Configuration.GetSection("ExchangeRateSettings"))
    .AddLogging(configure => configure
        .AddConsole() // Log to console for simplicity
        .SetMinimumLevel(LogLevel.Information))
    .AddMemoryCache()
    .AddHttpClient<IExchangeRateProvider, CnbExchangeRateProvider>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Exchange Rate API", Version = "v1" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();