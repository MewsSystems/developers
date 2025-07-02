using ExchangeRateUpdater.API.Endpoints.V1.ExchangeRates.Models;
using ExchangeRateUpdater.Core.UseCases.CommonModels;
using ExchangeRateUpdater.Core.UseCases.Queries.GetExchangeRates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.API.Endpoints.V1.ExchangeRates
{
	[Route("/v1/ExchangeRates/")]
	public class ExchangeRatesController : ApiControllerBase
	{
		private readonly IGetExchangeRateQuery _query;

		public ExchangeRatesController(ILogger<ExchangeRatesController> logger, IGetExchangeRateQuery query) : base(logger)
		{
			_query = query;
		}

		/// <summary>
		/// Get the currency exchange rates.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Produces("application/json")]
		[SwaggerResponse(200, "Get the list of exchanges for the specific currency.")]
		[SwaggerResponse(400, "Bad request.")]
		public async Task<ActionResult<IEnumerable<ExchangeRate>>> Get(GetExchangeRateModel model)
		{
			var response = await this._query.ExecuteAsync(model.ToQueryRequest());
			return Ok(response.Rates);
		}
	}
}
