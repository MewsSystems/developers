using Common.Configuration;
using Core.Parser;
using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Logging;

namespace Core.Client
{
    /// <summary>
    /// Common client logic
    /// </summary>
    public abstract class BaseClient
    {
        internal readonly ILogger<BaseClient> _logger;
        internal readonly IConfigurationWrapper _configurationWrapper;
        internal readonly IHttpWrapper _httpWrapper;
        internal readonly IResponseParser _responseParser;
        public BaseClient(ILogger<BaseClient> logger, IConfigurationWrapper configurationWrapper, IHttpWrapper httpWrapper, IResponseParser responseParser)
        {
            _logger = logger;
            _configurationWrapper = configurationWrapper;
            _httpWrapper = httpWrapper;
            _responseParser = responseParser;
        }
    }
}
