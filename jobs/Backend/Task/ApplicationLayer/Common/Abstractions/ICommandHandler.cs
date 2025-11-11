using MediatR;

namespace ApplicationLayer.Common.Abstractions;

/// <summary>
/// Handler interface for commands that return a result.
/// </summary>
/// <typeparam name="TCommand">The command type to handle</typeparam>
/// <typeparam name="TResponse">The type of response returned</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
