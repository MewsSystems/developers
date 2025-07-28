using Mews.ExchangeRateUpdater.API.Dto;
using Mews.ExchangeRateUpdater.Application;
using Mews.ExchangeRateUpdater.Application.Exceptions;
using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Mews.ExchangeRateUpdater.Infrastructure;
using Mews.ExchangeRateUpdater.Infrastructure.Exceptions;
using Mews.ExchangeRateUpdater.Infrastructure.Logging;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.With<TraceIdEnricher>()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] (TraceId: {TraceId}) {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting API");
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Serilog
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });
    
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

    var cnbUrl = builder.Configuration["Cnb:BaseUrl"];
    if (string.IsNullOrWhiteSpace(cnbUrl))
        throw new InvalidOperationException("Missing CNB base URL configuration (Cnb:BaseUrl).");
    
    // DI
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices($"Data Source={dbPath}", cnbUrl);

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Ensure DB is created
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated(); // Safe to run always
    }
    
    // Middleware for Serilog global error handling
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
            var traceId = context.Items["TraceId"]?.ToString();

            logger.LogError(exception, "Unhandled exception (TraceId: {TraceId})", traceId);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                error = "An unexpected error occurred.",
                traceId
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        });
    });

    // Middleware for Serilog TraceId log context
    app.Use(async (context, next) =>
    {
        var traceId = Guid.NewGuid().ToString();
        context.Items["TraceId"] = traceId;

        using (Serilog.Context.LogContext.PushProperty("TraceId", traceId))
        {
            await next();
        }
    });
    
    // Middleware to add TraceId to response headers
    app.Use(async (context, next) =>
    {
        var traceId = context.Items["TraceId"]?.ToString();
        if (!string.IsNullOrEmpty(traceId))
        {
            context.Response.Headers.Append("X-Trace-Id", traceId);
        }

        await next();
    });
    
    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI();

    // Endpoints
    app.MapGet("/rates", async (
            HttpContext ctx,
            IGetExchangeRatesUseCase useCase,
            [FromQuery] string[] currencies,
            CancellationToken ct) =>
        {
            try
            {
                var input = currencies.Select(c => new Currency(c)).ToList();

                var result = await useCase.ExecuteAsync(input, ct);

                var dto = result.Select(r => new ExchangeRateDto(
                    r.SourceCurrency.Code,
                    r.TargetCurrency.Code,
                    r.Value
                ));

                return Results.Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = $"Invalid currency code: {ex.Message}" });
            }
            catch (NoDataForTodayException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
        })
        .WithName("GetExchangeRates")
        .WithSummary("Get exchange rates")
        .WithDescription("Returns today's exchange rates for the given currency codes (e.g. USD, EUR). Fails if any code is invalid or if today's data is not available.")
        .WithOpenApi();

    app.MapPut("/rates", async (
            [FromServices] IFetchExchangeRatesUseCase useCase,
            [FromBody] UpdateRatesRequest request,
            CancellationToken ct) =>
        {
            try
            {
                await useCase.ExecuteAsync(ct, request.Force);
                return Results.Ok(new { status = "Rates updated successfully" });
            }
            catch (CnbServiceException ex)
            {
                return Results.Problem(statusCode: 503, title: "External service error", detail: ex.Message);
            }
            catch (Exception ex) when (ex is RatesAlreadyExistException or EmptyRatesFetchedException)
            {
                return Results.BadRequest(new { error = ex.Message });
            }            
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
        .WithName("UpdateExchangeRates")
        .WithSummary("Upserts today's exchange rates")
        .WithDescription("Fetches today's rates from CNB and stores them. Use 'force = true' to override existing data.")
        .WithOpenApi();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "API service terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}