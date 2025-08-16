using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRateUpdater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates([FromQuery] List<string> currencyCodes )
        {
            try
            {
                var requestedCurrencies = new List<Currency>();
                foreach (var code in currencyCodes)
                {
                    if (code.Count() > 3) // format check
                    {
                        return BadRequest($"Invalid Currency Code format '{code}', please use Three-letter ISO 4217 code of the currency.");
                    }
                    requestedCurrencies.Add(new Currency(code));
                }

                var rates = await _exchangeRateProvider.GetExchangeRates(requestedCurrencies);

                if (rates.Count() == 0 || rates is null) // empty check
                {
                    foreach (var code in currencyCodes)
                    {
                        Console.WriteLine($"Currency '{code}' not found");  
                    }

                    return NotFound($"Currency '{currencyCodes.FirstOrDefault()}' not found"); 
                }
                else
                {
                    Console.WriteLine($"Successfully retrieved '{rates.Count()}' exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine($"{rate?.Amount} {rate?.SourceCurrency.Code} = {rate?.Rate} {rate?.TargetCurrency.Code}");
                    }

                    return Ok(rates);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
