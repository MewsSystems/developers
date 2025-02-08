using JetBrains.Annotations;

namespace ExchangeRateUpdater.Api;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Message { get; set; } = string.Empty;
}

public static class ApiResponse
{
    public static ApiResponse<T> Successful<T>(T data) => new() { Success = true, Data = data };

    public static ApiResponse<T> Failure<T>(string message) => new() { Success = false, Message = message };
}