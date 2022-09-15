using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebApi.Controllers;

/// <summary>
/// The endpoint for exchange rates management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExchangeRateController : ControllerBase
{
  private readonly ExchangeRateProviderCzechNationalBankService _exchangeRateProviderCzechNationalBankService;
  private readonly ILogger<ExchangeRateController> _logger;
  
  public ExchangeRateController(ExchangeRateProviderCzechNationalBankService exchangeRateProviderCzechNationalBankService, ILogger<ExchangeRateController> logger)
  {
    _exchangeRateProviderCzechNationalBankService = exchangeRateProviderCzechNationalBankService;
    _logger = logger;
  }
  
  /// <summary>
  /// It returns exchange rates among the specified currencies that are defined in advance for the test purposes.
  /// </summary>
  /// <returns></returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRatesTest()
  {
    try
    {
      List<ExchangeRate> rates = (await _exchangeRateProviderCzechNationalBankService.GetExchangeRates()).ToList();
      
      Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
      foreach (ExchangeRate rate in rates)
      {
        Console.WriteLine(rate.ToString());
      }
      
      return Ok(rates);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
      
      return BadRequest(ex);
    }
  }
  
  /// <summary>
  /// It returns exchange rates among the specified currencies that are defined by the source.
  /// </summary>
  /// <returns></returns>
  [HttpGet("rates")]
  public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency>? currencies)
  {
    return Ok(await _exchangeRateProviderCzechNationalBankService.GetExchangeRates(currencies));
  }
}