using DomainLayer.Common;
using REST.Response.Models.Common;

namespace REST.Response.Converters;

/// <summary>
/// Extension methods for converting Result objects to ApiResponse objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a Result to an ApiResponse.
    /// </summary>
    public static ApiResponse ToApiResponse(this Result result, string? successMessage = null)
    {
        if (result.IsSuccess)
        {
            return ApiResponse.Ok(successMessage);
        }

        return ApiResponse.Fail(result.Error ?? "An error occurred", 400);
    }

    /// <summary>
    /// Converts a Result&lt;T&gt; to an ApiResponse&lt;T&gt; using a converter function.
    /// </summary>
    public static ApiResponse<TResponse> ToApiResponse<TDto, TResponse>(
        this Result<TDto> result,
        Func<TDto, TResponse> converter,
        string? successMessage = null)
    {
        if (result.IsSuccess && result.Value != null)
        {
            var responseData = converter(result.Value);
            return ApiResponse<TResponse>.Ok(responseData, successMessage);
        }

        if (result.IsSuccess && result.Value == null)
        {
            return ApiResponse<TResponse>.NotFound("Resource not found");
        }

        return ApiResponse<TResponse>.BadRequest(
            result.Error ?? "An error occurred",
            ErrorResponse.Create(result.Error ?? "An error occurred")
        );
    }

    /// <summary>
    /// Converts a Result&lt;IEnumerable&lt;T&gt;&gt; to an ApiResponse&lt;IEnumerable&lt;T&gt;&gt; using a converter function.
    /// </summary>
    public static ApiResponse<IEnumerable<TResponse>> ToApiResponse<TDto, TResponse>(
        this Result<IEnumerable<TDto>> result,
        Func<TDto, TResponse> converter,
        string? successMessage = null)
    {
        if (result.IsSuccess && result.Value != null)
        {
            var responseData = result.Value.Select(converter);
            return ApiResponse<IEnumerable<TResponse>>.Ok(responseData, successMessage);
        }

        if (result.IsSuccess && result.Value == null)
        {
            return ApiResponse<IEnumerable<TResponse>>.Ok(Enumerable.Empty<TResponse>(), successMessage);
        }

        return ApiResponse<IEnumerable<TResponse>>.BadRequest(
            result.Error ?? "An error occurred",
            ErrorResponse.Create(result.Error ?? "An error occurred")
        );
    }

    /// <summary>
    /// Converts a Result&lt;T&gt; to an ApiResponse&lt;T&gt; when no conversion is needed (T is already the response type).
    /// </summary>
    public static ApiResponse<T> ToApiResponse<T>(
        this Result<T> result,
        string? successMessage = null)
    {
        if (result.IsSuccess && result.Value != null)
        {
            return ApiResponse<T>.Ok(result.Value, successMessage);
        }

        if (result.IsSuccess && result.Value == null)
        {
            return ApiResponse<T>.NotFound("Resource not found");
        }

        return ApiResponse<T>.BadRequest(
            result.Error ?? "An error occurred",
            ErrorResponse.Create(result.Error ?? "An error occurred")
        );
    }
}
