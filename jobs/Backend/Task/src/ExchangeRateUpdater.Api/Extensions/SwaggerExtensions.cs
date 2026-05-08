using ExchangeRateUpdater.Api.Authorization;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExchangeRateUpdater.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
        private static string AppName => "ExchangeRate API";
        private static string AssemblyVersion => $"v{Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString()}";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                //API information and description
                options.SwaggerDoc(AssemblyVersion, new OpenApiInfo
                {
                    Version = AssemblyVersion,
                    Title = AppName,
                    Description = "Return exchange rates among the specified currencies that are defined by the source",
                    Contact = new OpenApiContact
                    {
                        Name = "Javier Val",
                        Url = new Uri("https://www.linkedin.com/in/javier-val-lista-ing-software/")
                    }
                });

                // Api Key
                options.AddSecurityDefinition("ClientApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = ApiKeyConstants.ApiKeyHeaderName,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "ClientApiKey"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

                //Configuring Swagger to use the documentation XML file
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerInApplication(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"/swagger/{AssemblyVersion}/swagger.json", $"{AppName} - {AssemblyVersion}");
                s.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
