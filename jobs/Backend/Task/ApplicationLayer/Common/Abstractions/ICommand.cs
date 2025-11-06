using DomainLayer.Common;
using MediatR;

namespace ApplicationLayer.Common.Abstractions;

/// <summary>
/// Marker interface for commands that modify state and return a result.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the command</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// Marker interface for commands that modify state without returning a value.
/// </summary>
public interface ICommand : IRequest<Result>
{
}
