namespace DataLayer.Entities;

public class ErrorLog
{
    public long Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
    public int? UserId { get; set; }
    public string? AdditionalData { get; set; }

    // Navigation properties
    public User? User { get; set; }
}
