using ExchangeRateUpdaterModels.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;

/// <summary>
/// Service for retrieving exchange rates from the CNB API.
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class CNBExchangeRetreivalController : ControllerBase
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CNBExchangeRetreivalController"/> class.
    /// </summary>
    
    public CNBExchangeRetreivalController(HttpClient httpClient)
    {
        if(httpClient == null)
        {
            _httpClient = new HttpClient();
        }
        else
        {
            _httpClient = httpClient;
        }
    }
    /// <summary>
    /// Asynchronously gets the exchange rates from the Czech National Bank API.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ExchangeRateModel"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> GetRatesAsync()
    {
        List<ExchangeRateModel> exchangeRates = new List<ExchangeRateModel>();

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://api.cnb.cz/cnbapi/exrates/daily");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            foreach (var rate in json["rates"])
            {
                exchangeRates.Add(new ExchangeRateModel(
                    new CurrencyModel(rate["currencyCode"].ToString()),
                    new CurrencyModel("CZK"), // Assuming target currency is CZK
                    decimal.Parse(rate["rate"].ToString())
                ));
            }
            return Ok(exchangeRates);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error fetching exchange rates from CNB API");
            return NoContent();
        }
    }
}
