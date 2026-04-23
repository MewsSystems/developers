namespace ExchangeRateUpdater.Application.Handlers.QueryHandlers.Abstract;

public interface IAsyncQueryHandler<TResult>
{
    Task<TResult> HandleAsync();
}

public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : IQuery
{
    Task<TResult> HandleAsync(TQuery query);
}
