using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/api.txt", rollingInterval: RollingInterval.Day)
    // .WriteTo.Elasticsearch(...) // Configure Elasticsearch sink if needed
    .ReadFrom.Configuration(ctx.Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>(serviceProvider =>
    new ExchangeRateProvider(
        serviceProvider.GetRequiredService<IHttpClientFactory>(),
        builder.Configuration,
        serviceProvider.GetRequiredService<ILogger<ExchangeRateProvider>>()));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks services and configure a custom check
builder.Services.AddHealthChecks()
    .AddCheck("app", () => HealthCheckResult.Healthy("The API is up and running."))
    .AddCheck<ExternalApiHealthCheck>("cnb_api", null, new[] { "external_api" });

builder.Services.AddTransient<ExternalApiHealthCheck>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.MapGet("/rates/{currencyCode}", async (string currencyCode, IExchangeRateProvider provider, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Fetching exchange rates for {CurrencyCode}", currencyCode);
        var rate = await provider.GetExchangeRate(currencyCode);
        return Results.Ok(rate);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error fetching exchange rates for {CurrencyCode}", currencyCode);
        return Results.Problem(detail: "An error occurred while fetching exchange rates.");
    }
})
.WithName("GetExchangeRates")
.AddEndpointFilter(async (context, next) =>
{
    var currencyCode = context.GetArgument<string>(0);
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrEmpty(currencyCode) || currencyCode.Length != 3)
    {
        errors.Add(nameof(currencyCode), ["Currency code must be exactly 3 characters."]);
        return Results.ValidationProblem(errors);
    }

    return await next(context);
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = System.Text.Json.JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            });
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});

app.Run();