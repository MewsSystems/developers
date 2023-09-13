using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ExchangeRateUpdater.Infrastructure.Providers;
using RestSharp;

namespace ExchangeRateUpdater.Infrastructure.Clients
{
    [ExcludeFromCodeCoverage]
    internal abstract class RestClientBase : IDisposable
    {
        private readonly RestClient _restClient;
        private readonly IMonitorProvider _monitorProvider;

        protected RestClientBase(HttpClient httpClient, IMonitorProvider monitorProvider)
        {
            _restClient = new RestClient(httpClient);
            _monitorProvider = monitorProvider;
        }

        protected abstract string ClientName { get; }

        public virtual async Task<RestResponse<T>> ExecuteGetAsync<T>(RestRequest request, string metricName)
        {
            var response = await _monitorProvider.ExecuteActionAsync(async () => await _restClient.ExecuteGetAsync<T>(request),
            metricName, 
            ClientName, 
            MethodBase.GetCurrentMethod()?.Name);

            return response;
        }

        public void Dispose()
        {
            _restClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
