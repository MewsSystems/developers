using Mews.ExchangeRateMonitor.Common.Domain.Results;
using Microsoft.AspNetCore.Http;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Mews.ExchangeRateMonitor.Common.API.Extensions;

public static class EndpointResultsExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Results.Problem();
        }

        return CreateProblem(errors);
    }

    private static IResult CreateProblem(List<Error> errors)
    {
        var statusCode = errors.First().Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,

            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status400BadRequest,
            ErrorType.Custom => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        return Results.ValidationProblem(errors.ToDictionary(k => k.Code, v => new[] { v.Description }),
            statusCode: statusCode);
    }
}