using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ExchangeRateUpdater.Api.Auth;

internal class AuthEndpointFilter : IEndpointFilter
{
    private readonly static object? _authFailResult = TypedResults.Unauthorized();

    private readonly IOptionsMonitor<AuthSettings> _authSettings;
    public AuthEndpointFilter(IOptionsMonitor<AuthSettings> authSettings)
    {
        _authSettings = authSettings ?? throw new ArgumentNullException(nameof(authSettings));
    }
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        bool authorized = ValidateApiKey(context.HttpContext.Request);

        return !authorized ? _authFailResult : await next(context);
    }

    private bool ValidateApiKey(HttpRequest request)
    {
        if (!request.Headers.TryGetValue(
            "X-Api-Key",
            out StringValues providedApiHeader))
        {
            return false;
        }

        string providedKey = providedApiHeader.ToString();

        bool authorized =
            providedKey == _authSettings.CurrentValue.ApiKey
            && !string.IsNullOrWhiteSpace(_authSettings.CurrentValue.ApiKey);

        return authorized;
    }
}
