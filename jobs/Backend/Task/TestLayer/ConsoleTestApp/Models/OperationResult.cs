namespace ConsoleTestApp.Models;

public class OperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
