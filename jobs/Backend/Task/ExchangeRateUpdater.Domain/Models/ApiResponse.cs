namespace ExchangeRateUpdater.Domain.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}
