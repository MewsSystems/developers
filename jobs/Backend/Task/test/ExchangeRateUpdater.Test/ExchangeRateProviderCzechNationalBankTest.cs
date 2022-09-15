using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.CoreModels;
using ExchangeRateUpdater.Service;
using ExchangeRateUpdater.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ExchangeRateUpdater.Test;

public class ExchangeRateProviderCzechNationalBankTest
{
  private static readonly ExchangeRateProviderSettings Settings = new()
  {
    ExchangeRatesSourceUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml",
    DefaultCurrencyCode = "CZK"
  };
  
  private static readonly ExchangeRateProviderCzechNationalBankService ExchangeRateProvider =
    new(Settings, null!, null!, null!);
  
  [Fact]
  public async Task ServiceTest()
  {
    List<ExchangeRate> rates = (await ExchangeRateProvider.GetExchangeRates()).ToList();

    Assert.NotNull(rates);
    Assert.Contains(rates, x => x.SourceCurrency.Code.ToLowerInvariant().Equals("EUR".ToLowerInvariant()));
    Assert.True(rates.Count(x => !x.TargetCurrency.Code.ToLowerInvariant().Equals(Settings.DefaultCurrencyCode.ToLowerInvariant())) == 0);
  }
  
  [Fact]
  public async Task ControllerTest()
  {
    ExchangeRateController controller = new ExchangeRateController(ExchangeRateProvider, null!);
    ActionResult<IEnumerable<ExchangeRate>> ratesResult = await controller.GetExchangeRates(null);
    
    ActionResult<IEnumerable<ExchangeRate>> result = Assert.IsType<ActionResult<IEnumerable<ExchangeRate>>>(ratesResult);
    OkObjectResult apiResponse = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
    Assert.Equal(200, apiResponse.StatusCode);
  }
}