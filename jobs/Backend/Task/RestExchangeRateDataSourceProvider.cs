using System.Net;
using log4net;
using RestSharp;

namespace ExchangeRateUpdater;

public class RestExchangeRateDataSourceProvider : IExchangeRateDataSourceProvider
{
    private readonly string _url;
    private readonly ILog _log;

    public RestExchangeRateDataSourceProvider(string url)
    {
        _url = url;
        _log = LogManager.GetLogger(GetType());
    }

    public string Get()
    {
        var client = new RestClient(_url);
        var request = new RestRequest();
        var response = client.Get(request);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            _log.Warn($"An error occurred while downloading exchange rate from {_url}. Status code is {response.StatusCode}.", response.ErrorException);
        }

        return response.Content;
    }
}