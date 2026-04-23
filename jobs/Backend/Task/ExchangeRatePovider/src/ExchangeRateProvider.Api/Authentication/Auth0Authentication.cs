using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ExchangeRateProvider.Api.Authentication;

/// <summary>
/// Provides extension methods for configuring Auth0 JWT authentication.
/// </summary>
public static class Auth0Authentication
{
    /// <summary>
    /// Adds Auth0 JWT Bearer authentication to the service collection with token validation parameters.
    /// </summary>
    /// <param name="services">The service collection to add authentication services to.</param>
    /// <param name="configuration">The configuration containing Auth0 settings (Authority and Audience).</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddAuth0Authentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var authority = configuration["Auth0:Authority"]!;
                var audience = configuration["Auth0:Audience"]!;

                options.Authority = authority;
                options.Audience = audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority.TrimEnd('/'),
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    NameClaimType = "sub"
                };
            });

        return services;
    }
}