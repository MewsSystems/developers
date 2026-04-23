using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Api.Presentation.Contracts.Responses;

/// <summary>
/// Standard API response wrapper that contains either data (success) or error information (failure).
/// </summary>
public sealed class ApiResponse<T>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorCode { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; init; }

    [JsonIgnore]
    public bool IsSuccess => ErrorCode is null;

    private ApiResponse() { }

    public static ApiResponse<T> Success(T data) => new() { Data = data };

    public static ApiResponse<T> Failure(string errorCode, string errorMessage) =>
        new() { ErrorCode = errorCode, ErrorMessage = errorMessage };
}
