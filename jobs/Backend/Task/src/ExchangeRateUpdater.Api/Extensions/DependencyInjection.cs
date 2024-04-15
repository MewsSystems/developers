using ExchangeRateUpdater.Api.Common;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for adding api services to the service collection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.UseDateOnlyTimeOnlyStringConverters();
                options.DescribeAllParametersInCamelCase();

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ExchangeRateUpdater.Api.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ExchangeRateUpdater.Application.xml"));
            });
            services.AddFluentValidationRulesToSwagger();

            services.AddHealthChecks();
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddApiVersioning()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
