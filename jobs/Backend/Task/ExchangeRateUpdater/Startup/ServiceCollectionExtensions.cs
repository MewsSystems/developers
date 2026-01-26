using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Business.Processors;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Mapping;
using ExchangeRateUpdater.Models.Configuration;
using ExchangeRateUpdater.Models.CzechNationalBank;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;
using ExchangeRateUpdater.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace ExchangeRateUpdater.Startup;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeRateUpdater.Docs", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        services.AddTransient<IExchangeRateMapper<RecordedRate>, CzechNationalBankResponseMapper>();
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddTransient<IExchangeRateProcessor, CzechNationalBankProcessor>();
    }

    public static void RegisterConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddOptionsWithValidateOnStart<CzechNationalBankClientOptions>("Integrations:ExchangeRateApis:CzechNationalBank");
        builder.Services.Configure<ExchangeRateConfigurationOptions>(builder.Configuration.GetSection("ExchangeRateConfiguration"));
    }

    public static void RegisterHttpClients(this IServiceCollection service, IConfiguration configuration)
    { 
        service.AddHttpClient<IExchangeRateClient<Response>, CzechNationalBankExchangeRateClient>( client =>
        {
            var options = configuration.GetSection("Integrations:ExchangeRateApis:CzechNationalBank").Get<CzechNationalBankClientOptions>();
                client.BaseAddress = new Uri($"https://{options.BaseUri}");
                client.Timeout = TimeSpan.FromMilliseconds(options.TimeoutMs);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
    }

    public static void RegisterValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<string[]>, ExchangeRateCurrencyCodeValidator>();
    }
}