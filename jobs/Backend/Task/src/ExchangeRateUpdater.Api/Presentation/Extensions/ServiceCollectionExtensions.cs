using Asp.Versioning;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Presentation.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(e => e.Value!.Errors.Select(err => err.ErrorMessage))
                        .ToArray();

                    var response = ApiResponse<object>.Failure(
                        ErrorCodes.ValidationFailed,
                        string.Join("; ", errors));

                    return new BadRequestObjectResult(response);
                };
            });

        return services;
    }
}
