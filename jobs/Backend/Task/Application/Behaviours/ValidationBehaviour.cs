using Domain.Errors.Base;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Behaviours;

internal sealed class ValidationBehaviour<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await Validate(request);
        if (validationResult.IsFailed)
        {
            var result = new TResponse();

            foreach (var reason in validationResult.Reasons)
            {
                result.Reasons.Add(reason);
            }

            return result;
        }

        return await next();
    }

    private Task<Result> Validate(TRequest request)
    {
        var context = new ValidationContext<TRequest>(request);
        Error[] errors = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailures => validationFailures is not null)
            .Select(failure => new ValidationError(failure.ErrorMessage))
            .Distinct()
            .ToArray();

        return Task.FromResult(Result.Fail(errors));
    }
}
