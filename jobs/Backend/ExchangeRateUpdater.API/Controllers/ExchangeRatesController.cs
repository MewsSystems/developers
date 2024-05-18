using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.Application.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace ExchangeRateUpdater.API.Controllers
{
    [ApiController]
    [Route("api/rates/")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExchangeRatesController> _logger;

        public ExchangeRatesController(IMediator mediator, ILogger<ExchangeRatesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("")]
        public async Task<IActionResult> GetExchangeRates([FromQuery] GetExchangeRatesRequest request)
        {
            CurrencyValidator currencyValidator = new CurrencyValidator();
            ValidationResult result = currencyValidator.Validate(request);

            if (!result.IsValid)
            {
                return BadRequest("Invalid Currency Supplied");
            }

            // TODO: outsource this conversion
            var targetCurrencies = new List<Currency>();

            foreach (string c in request.Currencies) 
            {
                var currency = new Currency(c);

                targetCurrencies.Add(currency);
            }

            // duplicate currencies ? > remove

            var exchangeRates = await _mediator.Send(new GetExchangeRatesQuery 
            {
                TargetCurrencies = targetCurrencies
            });

            return Ok(exchangeRates);
        }

        public class CurrencyValidator : AbstractValidator<GetExchangeRatesRequest>
        {
            // TODO: to think how to store currencies better
            private List<string> ValidCurrencies = new List<string>
            {
                "CZK",
                "GBP",
                "USD"
            };

            public CurrencyValidator()
            {
                RuleForEach(x => x.Currencies).ChildRules(order =>
                {
                    order.RuleFor(x => x).Must(MustBeAValidCurrency)
                    .WithMessage("Invalid currency supplied");
                });
            }

            private bool MustBeAValidCurrency(string code)
            {
                bool isValid = ValidCurrencies.Contains(code);
                return isValid;
            }
        }
    }
}
