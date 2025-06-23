using FluentValidation;
using MediatR;

namespace ExchangeRateUpdater.Application.Behaviours
{
    /// <summary>
    /// In this class we collect and run all the validations, in order to check the validation errors and throw the validation exception.
    /// The IPipelineBehavior interface allows us to intercept the request before or after executing the handler and add additional behavior (such as validations).
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="validators"></param>
    public class ValidationBehaviour<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures =
                validationResults
                .Where(v => v.Errors.Count != 0)
                .SelectMany(v => v.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
