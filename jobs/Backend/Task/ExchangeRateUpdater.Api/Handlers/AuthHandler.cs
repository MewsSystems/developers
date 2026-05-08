using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Models;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace ExchangeRateUpdater.Api.Handlers;

public static class AuthHandler
{
    public static async Task<IResult> GenerateToken(
        ClientCredentials credentials,
        JwtSettings jwtSettings,
        IValidator<ClientCredentials> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(credentials, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(new
            {
                Success = false,
                Errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                })
            });
        }

        // Hardcoded check for demo purposes
        if (credentials is not { ClientId: "exchange-rate-client" or "exchange-rate-admin", 
                ClientSecret: "your-super-secret-key" })
        {
            return Results.Unauthorized();
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, credentials.ClientId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.Role, credentials.ClientId == "exchange-rate-admin" ? "Admin" : "User")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryInMinutes),
            signingCredentials: signingCredentials
        );

        return Results.Ok(new
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresIn = jwtSettings.ExpiryInMinutes
        });
    }
} 