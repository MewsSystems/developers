using Mews.ExchangeRateMonitor.Common.API.Abstractions;
using Mews.ExchangeRateMonitor.Common.API.Extensions;
using Mews.ExchangeRateMonitor.Common.API.RateLimiting;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;

public class GetExratesDailyEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.ExratesDailyRoute, Handle)
            .WithDescription($"Returns last valid data for selected day")
            .RequireRateLimiting(AppRateLimiting.GlobalRateLimitPolicy);
    }

    private static async Task<IResult> Handle(
        [AsParameters] GetExratesDailyRequest request,
        [FromServices] IGetExratesDailyHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(request, cancellationToken);
        if (response.IsError)
            return response.Errors.ToProblem();

        return Results.Ok(response.Value);
    }
}
