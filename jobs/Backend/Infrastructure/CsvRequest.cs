using Data;

namespace Infrastructure;

public abstract class CsvRequest : BaseApiRequest<string>
{
    public CsvRequest(HttpMethod httpMethod) : base()
    {
        HttpRequest = new HttpRequestMessage
        {
            Method = httpMethod
        };
    }

    public override async Task<string> ParseHttpResponse(HttpResponseMessage response, ILog<BaseHttpApiClient> logger)
    {
        var responseString = await response.Content.ReadAsStringAsync();

        logger.Info($"ParseHttpResponse - responseString: {responseString}");

        return responseString;
    }
}
