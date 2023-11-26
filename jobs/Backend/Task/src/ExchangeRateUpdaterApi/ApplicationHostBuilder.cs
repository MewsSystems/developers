using System;
using Adapter.Http.CNB;
using Adapter.Http.CNB.Repositories;
using Domain.Ports;
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
    private readonly ISettings _settings;

    protected Container Container;

    public ApplicationHostBuilder(string[] args, string applicationName, ISettings settings)
    {
        _args = args;
        _applicationName = applicationName;
        _settings = settings;
    }
    
    public IHost BuildHost()
    {
        return BuildHost(config => { });
    }

    public IHost BuildHost(Action<IWebHostBuilder> config)
    {
        Logger logger = SerilogConfiguration.Create(_applicationName);
        logger.Information("Logger created;");
            
        Container = new Container();
        Container.Options.ResolveUnregisteredConcreteTypes = true;

        var hostBuilder = Host.CreateDefaultBuilder(_args).ConfigureWebHostDefaults(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers();

                    services.AddSimpleInjector(Container, options =>
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
                });
                
                config(builder);
            }
        );

        var host = hostBuilder.Build().UseSimpleInjector(Container);

        Container.RegisterInstance(logger);
        
        // Register dependencies
        var cnbSettings = new CNBSettings
        {
            BaseAddress = _settings.ExchangeRatesBaseAddress
        };
        
        IExchangeRatesRepository exchangeRatesRepository = new ExchangeRatesRepository(cnbSettings, logger);
        
        RegisterDependencies(exchangeRatesRepository);

        return host;
    }

    public virtual void RegisterDependencies(IExchangeRatesRepository exchangeRatesRepository)
    {
        Container.RegisterInstance<IExchangeRatesRepository>(exchangeRatesRepository);
    }
}