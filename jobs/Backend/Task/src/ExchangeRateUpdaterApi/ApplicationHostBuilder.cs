using ExchangeRateUpdaterApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog.Core;
using SimpleInjector;

namespace ExchangeRateUpdaterApi;

public class ApplicationHostBuilder
{
    private readonly string[] _args;
    private readonly string _applicationName;

    public ApplicationHostBuilder(string[] args, string applicationName)
    {
        _args = args;
        _applicationName = applicationName;
    }

    public IHost BuildHost()
    {
        Logger logger = SerilogConfiguration.Create(_applicationName);
        logger.Information("Logger created;");
            
        var container = new Container();

        var hostBuilder = Host.CreateDefaultBuilder(_args).ConfigureWebHostDefaults(builder =>
            builder.ConfigureServices(services =>
            {
                services.AddControllers();
                        
                services.AddSimpleInjector(container, options =>
                {
                    options
                        .AddAspNetCore()
                        .AddControllerActivation();
                });
                        
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = _applicationName,
                        Version = "v1"
                    });
                });
            }).Configure(application =>
            {
                application.UseSwagger();
                application.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", _applicationName);
                });

                application.UseRouting();

                application.UseAuthorization();

                application.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            })
        );

        var host = hostBuilder.Build().UseSimpleInjector(container);

        container.RegisterInstance(logger);

        return host;
    }
}