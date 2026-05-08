using Microsoft.AspNetCore.OutputCaching;

namespace ExchangeRateUpdater.Api.Handlers;

public static class CacheHandler
{
    public static async Task<IResult> PurgeCache(
        IOutputCacheStore cache,
        CancellationToken cancellationToken)
    {
        try
        {
            await cache.EvictByTagAsync("exchange-rates", cancellationToken);
            
            return Results.Ok(new
            {
                Success = true,
                Message = "Cache purged successfully"
            });
        }
        catch (Exception e)
        {
            return Results.Problem(
                title: "Error purging cache",
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}