using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Interface.Configuration
{
    public class CnbSettings
    {
        [Required(AllowEmptyStrings = false)]
        [Url]
        public string BaseUrl { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string GetExchangeRatesEndpoint { get; set; } = string.Empty;
    }
}
