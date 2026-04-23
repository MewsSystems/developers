using ExchangeRateUpdater.Api.Middleware;
using ExchangeRateUpdater.DependencyInjection;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from configuration (appsettings.json)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting web host");

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddExchangeRateServices();

    // Add Swagger services
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        // Enable Swagger UI in development
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Catch startup exceptions
    Log.Fatal(ex, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
