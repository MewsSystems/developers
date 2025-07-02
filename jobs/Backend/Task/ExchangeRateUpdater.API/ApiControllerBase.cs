using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.API
{
	[ApiController]
	public abstract class ApiControllerBase : ControllerBase
	{
		protected readonly ILogger _logger;

		protected ApiControllerBase(ILogger logger)
		{
			this._logger = logger;
		}
	}
}
