using ExchangeRateUpdater.Errors;
using FluentResults;
using System.Linq;

namespace ExchangeRateUpdater.Services.Handlers;

public static class ErrorHandler
{
    public static Result<T> Handle<T>(CnbErrorCode errorCode, string message)
    {
        return Result.Fail<T>(new Error(message)
            .WithMetadata("ErrorCode", errorCode));
    }

    public static CnbException ExtractError(IResultBase result)
    {
        var error = result.Errors.FirstOrDefault();
        if (error == null)
        {
            return new CnbException(CnbErrorCode.UnexpectedError, "Unknown error");
        }

        var errorCode = error.Metadata.TryGetValue("ErrorCode", out var code) && code is CnbErrorCode cnbCode
            ? cnbCode
            : CnbErrorCode.UnexpectedError;

        return new CnbException(errorCode, error.Message);
    }
}
