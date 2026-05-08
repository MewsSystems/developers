namespace Domain.Abstractions;

public interface IHttpClientService
{
    Task<T> GetJsonAsync<T>(string uri);
}
