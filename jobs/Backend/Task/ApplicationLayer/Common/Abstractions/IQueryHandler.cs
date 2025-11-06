using MediatR;

namespace ApplicationLayer.Common.Abstractions;

/// <summary>
/// Handler interface for queries that return data.
/// </summary>
/// <typeparam name="TQuery">The query type to handle</typeparam>
/// <typeparam name="TResponse">The type of data returned</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
