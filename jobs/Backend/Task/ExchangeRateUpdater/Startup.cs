using System.Threading.Tasks;
using ExchangeRateUpdater.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        var redisConnection = configuration.GetConnectionString("Redis");
        
        if (string.IsNullOrWhiteSpace(redisConnection))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
            });
        }

        services.ConfigureApplicationServices(configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            // Redirect root to swagger
            endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
        });
    }
}