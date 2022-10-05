using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Client
{
    /// <summary>
    /// Common client logic
    /// </summary>
    public abstract class BaseClient
    {
        internal readonly ILogger<BaseClient> _logger;
        internal readonly IConfiguration _configuration;
        internal readonly IHttpWrapper _httpWrapper;        

        public BaseClient(ILogger<BaseClient> logger, IConfiguration configuration, IHttpWrapper httpWrapper)
        {
            _logger = logger;
            _configuration = configuration;
            _httpWrapper = httpWrapper;            
        }
    }
}
