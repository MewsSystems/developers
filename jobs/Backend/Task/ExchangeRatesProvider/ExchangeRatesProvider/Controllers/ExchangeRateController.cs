using ExchangeRateProvider;
using ExchangeRatesProvider.Interfaces;
using ExchangeRatesProvider.Models;
using ExchangeRatesProvider.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ExchangeRatesProvider.Controllers
{
    [ApiController]
    [Route("")]
    public class ExchangeRateController : Controller
    {
        private IExchangeRateService ExchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            ExchangeRateService = exchangeRateService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRates()
        {
            if (ExchangeRateService != null)
            {
                try
                {
                    var response = await ExchangeRateService.GetExchangeRates();
                    if (response.statusCode != 200)
                    {
                        return BadRequest();
                    }

                    return View("Index", response.result);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }


            }
            return BadRequest();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string search)
        {

            try
            {
                var response = await ExchangeRateService.GetSearchResults(search);
                if (response.statusCode != 200)
                {
                    return BadRequest();
                }

                return View("Index", response.result);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
        [HttpGet("filteredTaskCurrencies")]
        public async Task<IActionResult> GetSelectedCurrencies()
        {
            if (ExchangeRateService != null)
            {
                try
                {
                    var response = await ExchangeRateService.GetSelectedCurrencies();
                    if (response.statusCode != 200)
                    {
                        return BadRequest();
                    }

                    return View("Index", response.result);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }


            }
            return BadRequest();
        }
    }
}
