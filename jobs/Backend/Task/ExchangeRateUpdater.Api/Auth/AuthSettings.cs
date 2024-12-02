using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Api.Auth;

internal class AuthSettings
{
    public const string ConfigSectionId = "Auth";

    [Required, MinLength(6)]
    public string? ApiKey { get; set; }
}
