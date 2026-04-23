using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Api.Presentation.Options;

public sealed class OpenApiOptions
{
    public const string SectionName = "OpenApi";

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Version { get; set; }

    [Required]
    public required string Description { get; set; }
}
