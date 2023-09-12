using System.Reflection;
using ExchangeRateUpdater.Infrastructure.Providers;
using RestSharp;

namespace ExchangeRateUpdater.Infrastructure.Clients
{
    internal abstract class RestClientBase
    {
        private readonly RestClient _restClient;
        private readonly IMonitorProvider _monitorProvider;

        protected RestClientBase(HttpClient httpClient, IMonitorProvider monitorProvider)
        {
            _restClient = new RestClient(httpClient);
            _monitorProvider = monitorProvider;
        }

        protected abstract string ClientName { get; }

        protected async Task<RestResponse<T>> ExecuteGetAsync<T>(RestRequest request, string metricName)
        {
            var response = await _monitorProvider.ExecuteActionAsync(async () => await _restClient.ExecuteGetAsync<T>(request),
            metricName, ClientName, MethodBase.GetCurrentMethod()?.Name);

            return response;
        }
    }
}
