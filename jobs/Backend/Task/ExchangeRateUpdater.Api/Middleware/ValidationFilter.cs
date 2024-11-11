using System.Net;
using FluentValidation;

namespace ExchangeRate.Api.Middleware;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        var argsToValidate = context.Arguments.OfType<T>().ToArray();

        if (validator == null || argsToValidate.Length <= 0) return await next.Invoke(context);

        var validationResults = await Task.WhenAll(argsToValidate.Select(arg => validator.ValidateAsync(arg)));
        var errors = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());

        if (errors.Count > 0)
            return Results.ValidationProblem(errors,
                statusCode: (int)HttpStatusCode.BadRequest);

        return await next.Invoke(context);
    }
}