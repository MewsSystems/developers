using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Errors.Base;

public class NotFoundError(string error) : Error(error), IError
{
    public static string Title = "No Data Found";

    public static ObjectResult Problem(string error) =>
        new ObjectResult(new ProblemDetails()
        {
            Title = Title,
            Status = 404,
            Detail = error
        });
}