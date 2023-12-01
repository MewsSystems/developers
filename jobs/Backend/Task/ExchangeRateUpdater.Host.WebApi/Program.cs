using Microsoft.Extensions.DependencyInjection.Extensions;
using ExchangeRateUpdater.Domain.Ports;
using Adapter.ExchangeRateProvider.InMemory;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using ExchangeRateUpdater.Host.WebApi.Configuration;

namespace ExchangeRateUpdater.Host.WebApi;

public class Program
{
    public static void Main()
    {
        var app = Configure().Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    public static WebApplicationBuilder Configure()
    {
        var builder = WebApplication.CreateBuilder();

        // Add services to the container.
        builder.Services.AddSingleton<IExchangeRateProviderRepository>(new ExchangeRateProviderRepositoryInMemory());
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        Log.Logger = SerilogConfiguration.SetupLogger();
        builder.Host.UseSerilog();
        
        Log.Logger.Information("Logger is configured, using Serilog");
        return builder;
    }
}