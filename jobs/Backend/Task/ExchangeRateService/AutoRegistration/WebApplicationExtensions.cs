using ExchangeRateService.Core;
using ExchangeRateService.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExchangeRateService.AutoRegistration;

internal static class WebApplicationExtensions
{
    internal static WebApplication MapApiEndpoints(this WebApplication app)
    {
        var apiGroup = app
            .MapGroup("api")
            .RequireCors("Default")
            .AddFluentValidationAutoValidation()
            .AddEndpointFilter<LoggingEndpointFilter>();

        var apiRouteTypes = typeof(IApiRoute).Assembly
            .GetClassesAssignableTo<IApiRoute>();

        foreach (var apiRoute in apiRouteTypes)
        {
            var route = (IApiRoute)Activator.CreateInstance(apiRoute)!;
            route.Register(apiGroup);
        }

        return app;
    }
}