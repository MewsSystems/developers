using Domain.Errors.Base;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Application.Extensions;

public static class FluentResultsExtensions
{
    public static ActionResult GetErrorResponse(this ResultBase result)
        => BuildErrorResponse(result);

    public static ActionResult GetErrorResponse(this Result result)
        => BuildErrorResponse(result);

    public static ActionResult GetErrorResponse<T>(this Result<T> result)
        => BuildErrorResponse(result);

    private static ActionResult BuildErrorResponse(ResultBase result)
    {
        if (result.HasError<ValidationError>())
        {
            var errors = GetErrors(result);
            return ValidationError.Problem(GetErrorMessage(errors));
        }

        if (result.HasError<NotFoundError>())
        {
            var errors = GetErrors(result);
            return NotFoundError.Problem(GetErrorMessage(errors));
        }

        throw new InvalidOperationException("No errors to process.");
    }

    private static IEnumerable<IError> GetErrors(ResultBase result)
    {
        List<IError> errors = new List<IError>();

        result.Errors.ForEach(error =>
        {
            errors.Add(error);
        });

        return errors;
    }

    private static string GetErrorMessage(IEnumerable<IError> errors)
        => string.Join(Environment.NewLine, errors.Select(e => e.Message));
}
