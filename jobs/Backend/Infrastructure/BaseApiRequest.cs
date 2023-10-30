using Data;

namespace Infrastructure;

public abstract class BaseApiRequest<T>
{
    public BaseApiRequest()
    {
        ParserDelegate = ParseHttpResponse;
    }

    public HttpRequestMessage HttpRequest { get; protected set; }
    public Func<HttpResponseMessage, ILog<BaseHttpApiClient>, Task<T>> ParserDelegate { get; set; }
    public Uri Uri { get; protected set; }
    public abstract Task<T> ParseHttpResponse(HttpResponseMessage response, ILog<BaseHttpApiClient> logger);
    public abstract void SetUri();
}
