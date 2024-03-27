using ExchangeRateProvider.BusinessLogic.IBusinessLogic;
using ExchangeRateProvider.DomainEntities;
using ExchangeRateProvider.DomainEntities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProvider.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateProviderController : ControllerBase
    {
        private readonly ICurrencyPairRates _currencyPairRates;
        public ExchangeRateProviderController(ICurrencyPairRates currencyPairRates)
        {
            _currencyPairRates = currencyPairRates;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<RatesDTO>> GetAsynct(DateTime dateTime, string language)
        {
            if (dateTime == DateTime.MinValue || language == null)
            {
                return BadRequest();
            }

            RequestModel requestModel = new RequestModel
            {
                DateTime = dateTime,
                Language = language
            };

            var rates = await _currencyPairRates.GetAllAsync(requestModel);

            if (rates == null || rates.Rates.Count == 0)
            {
                return NotFound();
            }

            return Ok(rates);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<RatesDTO>> GetAllAsync()
        {
            RequestModel requestModel = new RequestModel
            {
                DateTime = DateTime.MinValue
            };

            var rates = await _currencyPairRates.GetAllAsync(requestModel);

            if(rates == null)
            {
                return NotFound();
            }   

            return Ok(rates);
        }
    }
}
