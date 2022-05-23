using ExchangeRate.Client.Cnb;
using ExchangeRate.Service.Abstract;
using ExchangeRate.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace ExchangeRate.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ExchangeRateController : ControllerBase
	{
		private readonly IExchangeRateService _exchangeRateService;

		public ExchangeRateController(IExchangeRateService exchangeRateService)
		{
			_exchangeRateService = exchangeRateService;
		}

		/// <summary>
		/// Get CNB exchange rates
		/// </summary>
		/// <param name="apiType">CNB api type
		///     CnbTxt
		///     CnbXml
		/// </param>
		/// <returns>List exchange rates</returns>
		[ProducesResponseType(typeof(ActionResult<List<string>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status408RequestTimeout)]
		[ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		[HttpGet]
		[FeatureGate(nameof(FeatureFlags.EnableGetCnbExchangeRates))]
		public async Task<ActionResult<List<string>>> GetCnbExchangeRates(CnbConstants.ApiType apiType = CnbConstants.ApiType.CnbXml)
		{
			return Ok(await _exchangeRateService.GetExchangeRates(apiType));
		}
	}
}
