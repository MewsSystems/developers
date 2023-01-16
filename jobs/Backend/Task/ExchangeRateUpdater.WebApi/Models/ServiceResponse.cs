namespace ExchangeRateUpdater.WebApi.Models;

public class ServiceResponse<T>
{
    public T? Data { get; init; }
    public bool Success { get; init; } = true;
    public string? Message { get; set; } = string.Empty;
}