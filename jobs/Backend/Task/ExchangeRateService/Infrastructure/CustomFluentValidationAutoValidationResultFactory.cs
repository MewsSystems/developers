using FluentValidation.Results;
using Microsoft.AspNetCore.Http.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;
using SharpGrip.FluentValidation.AutoValidation.Shared.Extensions;

namespace ExchangeRateService.Infrastructure;

public partial class CustomFluentValidationAutoValidationResultFactory(
    ILogger<CustomFluentValidationAutoValidationResultFactory> logger) : IFluentValidationAutoValidationResultFactory
{
    private readonly ILogger<CustomFluentValidationAutoValidationResultFactory> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "{Method} {ApiUrl} did not started due to {ExceptionName}, {TraceIdentifier}.")]
    partial void LogValidationException(string method, string apiUrl, string exceptionName, string traceIdentifier);
    public IResult CreateResult(EndpointFilterInvocationContext context, ValidationResult validationResult)
    {
        var request = context.HttpContext.Request;
        LogValidationException(request.Method, request.GetDisplayUrl(), nameof(ValidationException), context.HttpContext.TraceIdentifier);
        return TypedResults.ValidationProblem(validationResult.ToValidationProblemErrors());
    }
}