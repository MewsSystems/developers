using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.WebApi
{
    public class Program
    {
        const string VERSION = "1";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appSettings = builder.Configuration.Get<AppSettings>();
            appSettings.EnsureIsOk();

            builder.Services
                .AddSwagger(
                    title: "ExchangeRateApi",
                    description: "API to handle Exchange Rates",
                    version: VERSION)
                .AddCarter()
                .AddAppServices(appSettings);            

            var app = builder.Build();

            app.UserErrorHandler(builder.Environment.IsDevelopment())
                .UseSwagger()
                .UseSwaggerUI()
                .UseRouting();

            app.MapCarter();

            app.Run();                           
        }
    }
}