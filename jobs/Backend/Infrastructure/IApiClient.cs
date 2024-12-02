namespace Infrastructure;

public interface IApiClient
{
    Task<T> ExecuteAsync<T>(BaseApiRequest<T> request);
}
