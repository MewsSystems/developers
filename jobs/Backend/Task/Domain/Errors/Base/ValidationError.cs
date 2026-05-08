using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Errors.Base;

public sealed class ValidationError : Error, IError
{
    public ValidationError(string error) : base(error) { }

    public static string Title = "Validation Error";

    public static ObjectResult Problem(string error) =>
        new ObjectResult(new ProblemDetails()
        {
            Title = Title,
            Status = 422,
            Detail = error
        });
}