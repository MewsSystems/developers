using Mews.ExchangeRate.API;
using Mews.ExchangeRate.API.ConfigurationExtensionMethods;
using Serilog;
using System.Diagnostics.CodeAnalysis;

Log.Logger = new LoggerConfiguration().
    CreateMewsBootstrapLogger(ExchangeRateApiConstants.ServiceName);

try
{
    Log.Information("starting {serviceName}",
        ExchangeRateApiConstants.ServiceName);

    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .AddMewsLoggingConfiguration(ExchangeRateApiConstants.ServiceName);

    builder.Services
        .AddMewsServices(builder.Configuration)
        .AddControllers();
    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger()
            .UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapHealthChecks($"/{ExchangeRateApiConstants.RoutePrefix}/healthcheck");
    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e,
        "An unhandled exception occurred during {serviceName} startup",
        ExchangeRateApiConstants.ServiceName);
}
finally
{
    Log.CloseAndFlush();
}

namespace Mews.ExchangeRate.API
{
    [ExcludeFromCodeCoverage(Justification = "Program.cs cannot be unit tested.")]
    public partial class Program
    {
        protected Program() { }
    }
}