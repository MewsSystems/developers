using System.Runtime.CompilerServices;
using ExchangeRatesApi.Models;
using ExchangeRatesService.Models;
using ExchangeRatesService.Providers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatesController : ControllerBase
{
    private readonly IRatesProvider _rateProvider;

    public RatesController(IRatesProvider rateProvider)
    {
        _rateProvider = rateProvider;
    }

    [HttpGet]
    [Route("exchange-rates")]
    public async IAsyncEnumerable<ExchangeRate> GetRates([FromQuery] List<Currency> codes,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var rates = _rateProvider.GetRatesAsync(codes, cancellationToken);
        await foreach (var rate in rates)
        {
            yield return new ExchangeRate(rate.ToString());
        }
    }
    
    /// <summary>
    /// Calculate reverse exchange rates base on fixed conversion to base currency
    /// https://learning.treasurers.org/resources/how-to-calculate-foreign-currency#:~:text=To%20convert%20from%20the%20base,
    /// exchange%20rate%20EUR%2FUSD%201.25.
    /// </summary>
    /// <param name="codes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("reverse-rates")]
    public async IAsyncEnumerable<ExchangeRate> GetReverseRates([FromQuery] List<Currency> codes,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var rates = _rateProvider.GetRatesReverseAsync(codes, 10, cancellationToken);
        await foreach (var rate in rates)
        {
            yield return new ExchangeRate(rate.ToString());
        }
    }
    
    public record ExchangeRate(string rate);
}