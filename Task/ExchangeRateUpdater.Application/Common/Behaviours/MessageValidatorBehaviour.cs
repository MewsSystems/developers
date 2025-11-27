namespace ExchangeRateUpdater.Application.Common.Behaviours;

using FluentValidation;
using Mediator;

public sealed class MessageValidatorBehaviour<TMessage, TResponse>(IEnumerable<IValidator<TMessage>> validators) : MessagePreProcessor<TMessage, TResponse>
    where TMessage : IMessage
{
    protected override async ValueTask Handle(TMessage message, CancellationToken cancellationToken)
    {
        foreach (var validator in validators)
        {
            await validator.ValidateAndThrowAsync(message, cancellationToken);
        }
    }
}
