namespace ApplicationLayer.DTOs.SystemHealth;

/// <summary>
/// DTO for error summary information.
/// </summary>
public class ErrorSummaryDto
{
    public string ErrorType { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public int OccurrenceCount { get; set; }
    public DateTimeOffset FirstOccurrence { get; set; }
    public DateTimeOffset LastOccurrence { get; set; }
    public List<string> AffectedProviders { get; set; } = new();
}
