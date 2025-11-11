namespace REST.Response.Models.Common;

/// <summary>
/// Generic API response wrapper that includes data payload.
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponse<T> : ApiResponse
{
    /// <summary>
    /// The data payload of the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ApiResponse<T> Ok(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            StatusCode = 200
        };
    }

    /// <summary>
    /// Creates a successful response with custom status code.
    /// </summary>
    public static ApiResponse<T> Ok(T data, int statusCode, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates an error response with no data.
    /// </summary>
    public new static ApiResponse<T> Fail(string message, int statusCode = 400, ErrorResponse? error = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Error = error ?? new ErrorResponse { Message = message },
            Data = default
        };
    }

    /// <summary>
    /// Creates a not found response.
    /// </summary>
    public static ApiResponse<T> NotFound(string message = "Resource not found")
    {
        return Fail(message, 404);
    }

    /// <summary>
    /// Creates a bad request response.
    /// </summary>
    public static ApiResponse<T> BadRequest(string message, ErrorResponse? error = null)
    {
        return Fail(message, 400, error);
    }

    /// <summary>
    /// Creates an unauthorized response.
    /// </summary>
    public static ApiResponse<T> Unauthorized(string message = "Unauthorized")
    {
        return Fail(message, 401);
    }

    /// <summary>
    /// Creates a forbidden response.
    /// </summary>
    public static ApiResponse<T> Forbidden(string message = "Forbidden")
    {
        return Fail(message, 403);
    }
}
