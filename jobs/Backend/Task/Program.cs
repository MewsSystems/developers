using ExchangeRateUpdater.Api;
using ExchangeRateUpdater.Infrastructure;

namespace ExchangeRateUpdater;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "Exchange Rate API",
                Version = "v1",
                Description = "API for fetching exchange rates from Czech National Bank",
                Contact = new()
                {
                    Name = "Exchange Rate Updater",
                    Url = new Uri("https://github.com/MewsSystems/developers")
                }
            });
        });

        builder.Services.AddExchangeRateProvider(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.MapExchangeRateEndpoints();

        app.Run();
    }
}
