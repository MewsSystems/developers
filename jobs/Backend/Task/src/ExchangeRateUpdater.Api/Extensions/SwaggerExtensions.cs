using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ExchangeRateUpdater.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Exchange Rate API",
                    Version = "v1",
                    Description = "API for retrieving currency exchange rates from the Czech National Bank",
                    Contact = new OpenApiContact
                    {
                        Name = "Exchange Rate Team",
                        Email = "support@example.com"
                    }
                });

            // Include XML comments
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);

            // Add operation filters for API versioning
            options.OperationFilter<SwaggerDefaultValues>();

            // Add examples using built-in functionality
            options.OperationFilter<ExamplesOperationFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        });

        return app;
    }
}

// Custom operation filter for API versioning
public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation,
        OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1752#issue-663991077
        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            var response = operation.Responses[responseKey];

            foreach (var contentType in response.Content.Keys)
                if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    response.Content.Remove(contentType);
        }
    }
}

// Custom operation filter for examples
public class ExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation,
        OperationFilterContext context)
    {
        // Add request examples
        foreach (var parameter in operation.Parameters)
        {
            // Exchange rate query example
            if (parameter.Name.Contains("currencyPair", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Example = new Microsoft.OpenApi.Any.OpenApiString("USD/CZK");
                parameter.Description += "\nExample: USD/CZK";
            }

            if (parameter.Name.Contains("date", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Example = new Microsoft.OpenApi.Any.OpenApiString("2023-11-15");
                parameter.Description += "\nExample: 2023-11-15 (ISO-8601 format)";
            }
        }

        // Add response examples
        foreach (var response in operation.Responses)
            if (response.Key == "200" || response.Key == "201")
            {
                foreach (var content in response.Value.Content)
                    if (content.Key.Contains("json", StringComparison.OrdinalIgnoreCase))
                    {
                        if (context.MethodInfo.Name.Contains("GetExchangeRate", StringComparison.OrdinalIgnoreCase))
                        {
                            content.Value.Example = new Microsoft.OpenApi.Any.OpenApiObject
                            {
                                ["date"] = new Microsoft.OpenApi.Any.OpenApiString("2023-11-15"),
                                ["currencyPair"] = new Microsoft.OpenApi.Any.OpenApiString("USD/CZK"),
                                ["rate"] = new Microsoft.OpenApi.Any.OpenApiFloat(22.123f),
                                ["source"] = new Microsoft.OpenApi.Any.OpenApiString("CNB")
                            };
                        }
                        else if (context.MethodInfo.Name.Contains("BatchRates", StringComparison.OrdinalIgnoreCase))
                        {
                            var ratesArray = new Microsoft.OpenApi.Any.OpenApiArray
                            {
                                new Microsoft.OpenApi.Any.OpenApiObject
                                {
                                    ["currencyPair"] = new Microsoft.OpenApi.Any.OpenApiString("USD/CZK"),
                                    ["rate"] = new Microsoft.OpenApi.Any.OpenApiFloat(22.123f)
                                },
                                new Microsoft.OpenApi.Any.OpenApiObject
                                {
                                    ["currencyPair"] = new Microsoft.OpenApi.Any.OpenApiString("EUR/CZK"),
                                    ["rate"] = new Microsoft.OpenApi.Any.OpenApiFloat(25.456f)
                                },
                                new Microsoft.OpenApi.Any.OpenApiObject
                                {
                                    ["currencyPair"] = new Microsoft.OpenApi.Any.OpenApiString("GBP/CZK"),
                                    ["rate"] = new Microsoft.OpenApi.Any.OpenApiFloat(29.789f)
                                }
                            };

                            content.Value.Example = new Microsoft.OpenApi.Any.OpenApiObject
                            {
                                ["date"] = new Microsoft.OpenApi.Any.OpenApiString("2023-11-15"),
                                ["rates"] = ratesArray,
                                ["source"] = new Microsoft.OpenApi.Any.OpenApiString("CNB")
                            };
                        }
                    }
            }
            else if (response.Key == "400")
            {
                foreach (var content in response.Value.Content)
                    if (content.Key.Contains("json", StringComparison.OrdinalIgnoreCase))
                    {
                        var errorsObject = new Microsoft.OpenApi.Any.OpenApiObject
                        {
                            ["Date"] = new Microsoft.OpenApi.Any.OpenApiArray
                            {
                                new Microsoft.OpenApi.Any.OpenApiString("Date must be ISO-8601 (yyyy-MM-dd)")
                            },
                            ["CurrencyPair"] = new Microsoft.OpenApi.Any.OpenApiArray
                            {
                                new Microsoft.OpenApi.Any.OpenApiString("Currency pair must be in format XXX/YYY using ISO-4217 codes")
                            }
                        };

                        content.Value.Example = new Microsoft.OpenApi.Any.OpenApiObject
                        {
                            ["status"] = new Microsoft.OpenApi.Any.OpenApiInteger(400),
                            ["title"] = new Microsoft.OpenApi.Any.OpenApiString("Validation Error"),
                            ["detail"] = new Microsoft.OpenApi.Any.OpenApiString("The request contains invalid parameters"),
                            ["errors"] = errorsObject
                        };
                    }
            }
    }
}