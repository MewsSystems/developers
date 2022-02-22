using System.Diagnostics;
using System.Net;
using log4net;
using RestSharp;

namespace ExchangeRateUpdater.DataSource;

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
        _log.Info($"Getting current rates from {_url}");

        var stopwatch = Stopwatch.StartNew();
        var client = new RestClient(_url);
        var request = new RestRequest();
        var response = client.Get(request);

        stopwatch.Stop();


        if (response.StatusCode != HttpStatusCode.OK)
        {
            _log.Warn($"An error occurred while downloading exchange rate from {_url}. Status code is {response.StatusCode}.", response.ErrorException);
        }

        _log.Info($"Current rates received in {stopwatch.ElapsedMilliseconds}ms");
        return response.Content;
    }
}