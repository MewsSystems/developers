using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.API
{
	[ApiController]
	public abstract class ApiControllerBase : ControllerBase
	{
		protected readonly ILogger logger;

		protected ApiControllerBase(ILogger logger)
		{
			this.logger = logger;
		}
	}
}
