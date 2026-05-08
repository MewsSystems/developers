using ExchangeRateUpdater.Api.Authorization;

namespace ExchangeRateUpdater.Api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IApiKeyValidation _apiKeyValidation;

        public ApiKeyMiddleware(RequestDelegate next, IApiKeyValidation apiKeyValidation)
        {
            _next = next;
            _apiKeyValidation = apiKeyValidation;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? userApiKey = context.Request.Headers[ApiKeyConstants.ApiKeyHeaderName];

            if (string.IsNullOrWhiteSpace(userApiKey)
                || !_apiKeyValidation.IsValidApiKey(userApiKey!))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.Append("www-authenticate", "Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}
