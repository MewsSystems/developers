using Microsoft.AspNetCore.Http.Features;

namespace ExchangeRate.Api.Configuration;

public static class ErrorHandlingConfiguration
{
    public static IServiceCollection ConfigureErrorHandling(this IServiceCollection services)
    {
        return services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });
    }

    public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(exceptionHandlerApp
            => exceptionHandlerApp.Run(async context
                => await Results.Problem()
                    .ExecuteAsync(context)));
    }
}