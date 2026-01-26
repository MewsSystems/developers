using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.Startup
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {

        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterConfigurations();
            builder.Services.RegisterValidators();
            builder.Services.AddControllers();
            builder.Services.RegisterHttpClients(builder.Configuration);
            builder.Services.AddServices();

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            { 
                app.MapSwagger();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.MapControllers();
            app.Run();
        }
    }
}
