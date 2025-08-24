using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using Mews.ExchangeRateMonitor.Common.Domain.Results;

namespace Mews.ExchangeRateMonitor.Common.Application.Extensions;

public static class ValidationExtensions
{
    public static List<string> ToFormattedErrorMessages(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            .ToList();
    }
    public static List<Error> ToDomainErrors(this ValidationResult validationResult, string errorPrefix)
    {
        return validationResult.Errors
            .Select(x => Error.Validation(errorPrefix, $"{x.PropertyName}:{x.ErrorMessage}"))
            .ToList();
    }


    public static void LogValidationErrors<T>(this ILogger<T> logger, ValidationResult validationResult, string contextMessage)
    {
        var validationErrors = validationResult.ToFormattedErrorMessages();
        logger.LogWarning("{ContextMessage}: {Errors}", contextMessage, string.Join(", ", validationErrors));
    }
}