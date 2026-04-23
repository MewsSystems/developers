using Mews.ExchangeRateUpdater.Application;
using Mews.ExchangeRateUpdater.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Mews.ExchangeRateUpdater.Api;

/// <summary>
/// Project startup.
/// </summary>
public class Startup
{
    private const string CorsPolicy = nameof(CorsPolicy);
    private const int LoggingBodyLogLimit = 1024;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// <see cref="IConfiguration"/>.
    /// </summary>
    private IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        
        _ = services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestBodyLogLimit = LoggingBodyLogLimit;
            logging.ResponseBodyLogLimit = LoggingBodyLogLimit;
        });

        _ = services.AddSwagger()
            .AddApplicationServices()
            .AddInfrastructureServices(Configuration)
            .AddCors(options => options.AddPolicy(CorsPolicy,
                    builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()));
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        _ = app.UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mews.ExchangeRateUpdater.Api v1");
                options.RoutePrefix = string.Empty;
                options.DisplayOperationId();
                options.DisplayRequestDuration();
                options.OAuthAppName("API Swagger UI");
            });

        if (!env.IsProduction())
        {
            _ = app.UseHttpLogging();
        }

        _ = app.UseHttpsRedirection()
            .UseRouting()
            .UseCors(CorsPolicy)
            .UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllers();
            });
    }
}
