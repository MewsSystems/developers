using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesService.Configuration;

public class CnbExchangeRateApiConfig
{
    [Url]
    public string ApiUrl { get; set; } = "https://api.cnb.cz";
}