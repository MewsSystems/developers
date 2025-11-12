using ExchangeRate.Api.Clients;
using ExchangeRate.Api.Configuration.Settings;
using ExchangeRate.Domain.Providers;
using ExchangeRate.Domain.Validators;
using FluentValidation;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace ExchangeRate.Api.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureTelemetry();
        builder.MapConfigurationSections();

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .ConfigureDependencies()
            .ConfigureResiliencePolicy()
            .ConfigureErrorHandling();
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapEndpoints();
        app.ConfigureExceptionMiddleware();
    }

    private static WebApplicationBuilder ConfigureTelemetry(this WebApplicationBuilder builder)
    {
        var telemetrySettings = builder.Configuration
            .GetSection(nameof(TelemetrySettings))
            .Get<TelemetrySettings>();

        var headers = new Dictionary<string, string>();
        if (telemetrySettings.TelemetryEndpointUseAuthentication)
            headers["X-Seq-ApiKey"] = telemetrySettings.TelemetryEndpointApiKey;

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(x =>
            {
                x.Endpoint = telemetrySettings.TelemetryEndpointUrl;
                x.Protocol = OtlpProtocol.HttpProtobuf;
                x.Headers = headers;
                x.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = nameof(ExchangeRate)
                };
            })
            .CreateLogger();

        builder.Services.AddSerilog();
        return builder;
    }

    private static IServiceCollection ConfigureDependencies(this IServiceCollection services)
    {
        return services
            .RegisterExchangeRateProviders()
            .RegisterValidators();
    }

    private static IServiceCollection RegisterExchangeRateProviders(this IServiceCollection services)
    {
        return services
            .AddKeyedTransient<IExchangeRateClient, CzechNationalBankClient>(ExchangeRateProviderType.Cnb);
    }

    private static IServiceCollection MapConfigurationSections(this WebApplicationBuilder builder)
    {
        return builder.Services
            .Configure<TelemetrySettings>(builder.Configuration.GetSection(nameof(TelemetrySettings)));
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssemblyContaining<CzechNationalBankProviderRequestValidator>();
    }
}