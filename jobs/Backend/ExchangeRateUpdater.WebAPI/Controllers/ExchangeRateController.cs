﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebAPI.Controllers
{
    public class ExchangeRateController : CustomBaseController
    {
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(ILogger<ExchangeRateController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetExchangeRates")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting Exchange Rates from Exchange Rate Controller");
            return Ok("Initial Exchange Rate Controller Creation");
        }
    }
}
