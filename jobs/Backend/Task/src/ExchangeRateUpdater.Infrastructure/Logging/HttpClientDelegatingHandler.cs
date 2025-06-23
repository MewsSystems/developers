using ExchangeRateUpdater.Application.Exceptions;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace ExchangeRateUpdater.Infrastructure.Logging
{
    /// <summary>
    /// Handler that centralizes logging and exception control operations in calls to external services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HttpClientDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpClientDelegatingHandler> _logger;

        public HttpClientDelegatingHandler(ILogger<HttpClientDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Sending request to {Url}", request.RequestUri);

                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Received a success response from {Url}", response.RequestMessage?.RequestUri);
                }
                else
                {
                    var httpContentString = await response.Content.ReadAsStringAsync(cancellationToken);

                    _logger.LogError(
                        "Received a non-success status code {StatusCode} from {Url}. Reason: {Reason}",
                        (int)response.StatusCode,
                        response.RequestMessage?.RequestUri,
                        httpContentString);

                    throw new BadGatewayException($"Error in external service provider. Reason: {response.ReasonPhrase}");
                }

                return response;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionRefused)
            {
                var hostWithPort = "-1";
                if (request.RequestUri != null)
                {
                    hostWithPort = request.RequestUri.IsDefaultPort
                        ? request.RequestUri.DnsSafeHost
                        : $"{request.RequestUri.DnsSafeHost}:{request.RequestUri.Port}";
                }

                _logger.LogCritical(ex, "Unable to connect to {Host}. Please check the configuration to ensure the correct URL for the service has been configured.", hostWithPort);

                return new HttpResponseMessage(HttpStatusCode.BadGateway)
                {
                    RequestMessage = request
                };
            }  
        }
    }
}
