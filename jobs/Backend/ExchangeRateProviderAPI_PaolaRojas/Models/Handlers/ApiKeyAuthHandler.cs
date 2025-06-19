using ExchangeRateProviderAPI_PaolaRojas.Models.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ExchangeRateProviderAPI_PaolaRojas.Models.Handlers
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKeyScheme";
        public string Scheme => DefaultScheme;
        public string ApiKeyHeaderName { get; set; } = "X-API-KEY";
    }

    public class ApiKeyAuthHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<ApiKeyOptions> apiKeyOptions) : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
    {
        private readonly string _configuredApiKey = apiKeyOptions.Value.Key;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Options.ApiKeyHeaderName, out var extractedApiKey))
                return Task.FromResult(AuthenticateResult.Fail("API Key missing"));

            if (string.IsNullOrWhiteSpace(_configuredApiKey) || !_configuredApiKey.Equals(extractedApiKey))
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));

            var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
            var identity = new ClaimsIdentity(claims, Options.Scheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}