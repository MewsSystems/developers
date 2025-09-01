using ExchangeRateUpdater.Abstractions.Contracts;
using ExchangeRateUpdater.Api.Mapping;
using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Provider.Cnb.Client;
using ExchangeRateUpdater.Provider.Cnb.Options;
using ExchangeRateUpdater.Services.Options;
using ExchangeRateUpdater.Services.Providers;
using ExchangeRateUpdater.Services.Refreshing;
using ExchangeRateUpdater.Services.Time;
using ExchangeRateUpdater.Api.Middleware;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddMemoryCache();

builder.Services.Configure<CnbOptions>(builder.Configuration.GetSection("Cnb"));
builder.Services.Configure<CnbRefreshOptions>(builder.Configuration.GetSection("CnbRefresh"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient<ICnbClient, CnbClient>();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddSingleton<IAppClock, AppClock>();
builder.Services.AddSingleton<IRatesStore, RatesStore>();

builder.Services.AddHostedService<CnbDailyRefreshService>();

builder.Services.Configure<HostOptions>(o =>
    o.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
