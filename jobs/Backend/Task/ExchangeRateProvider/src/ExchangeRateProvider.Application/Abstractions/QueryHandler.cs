namespace ExchangeRateProvider.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResult> where TQuery : class
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
