using MediatR;

namespace ApplicationLayer.Common.Abstractions;

/// <summary>
/// Marker interface for queries that retrieve data without modifying state.
/// </summary>
/// <typeparam name="TResponse">The type of data returned by the query</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
