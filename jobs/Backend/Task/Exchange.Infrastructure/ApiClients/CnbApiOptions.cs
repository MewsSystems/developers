using System.ComponentModel.DataAnnotations;

namespace Exchange.Infrastructure.ApiClients;

public class CnbApiOptions
{
    public const string SectionName = "CnbApi";
    [Required] [Url] public required string BaseAddress { get; set; }
    [Range(1, 100)] public required int TimeoutInSeconds { get; set; } = 60;
}