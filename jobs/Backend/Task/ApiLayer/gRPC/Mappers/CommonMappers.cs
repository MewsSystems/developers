using ApplicationLayer.DTOs.Common;
using gRPC.Protos.Common;
using DomainLayer.Common;

namespace gRPC.Mappers;

/// <summary>
/// Mappers for common types like pagination, errors, and operation results.
/// </summary>
public static class CommonMappers
{
    // ============================================================
    // PAGINATION
    // ============================================================

    public static PagingResponse ToProtoPaging<T>(PagedResult<T> pagedResult)
    {
        return new PagingResponse
        {
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            HasNext = pagedResult.HasNextPage,
            HasPrevious = pagedResult.HasPreviousPage
        };
    }

    // ============================================================
    // ERROR HANDLING
    // ============================================================

    public static ErrorDetails ToProtoError(Error error)
    {
        return new ErrorDetails
        {
            Code = error.Code,
            Message = error.Description
        };
    }

    public static ErrorDetails ToProtoError(string code, string message, params string[] validationErrors)
    {
        var error = new ErrorDetails
        {
            Code = code,
            Message = message
        };

        if (validationErrors?.Length > 0)
        {
            error.ValidationErrors.AddRange(validationErrors);
        }

        return error;
    }

    // ============================================================
    // OPERATION RESPONSE
    // ============================================================

    public static OperationResponse ToProtoOperationResponse(bool success, string message, Error? error = null)
    {
        var response = new OperationResponse
        {
            Success = success,
            Message = message
        };

        if (error != null)
        {
            response.Error = ToProtoError(error);
        }

        return response;
    }

    public static OperationResponse ToProtoOperationResponse<T>(Result<T> result, string successMessage)
    {
        if (result.IsSuccess)
        {
            return new OperationResponse
            {
                Success = true,
                Message = successMessage
            };
        }

        return new OperationResponse
        {
            Success = false,
            Message = result.Error ?? "Operation failed",
            Error = result.Error != null ? ToProtoError("OPERATION_ERROR", result.Error) : null
        };
    }
}
