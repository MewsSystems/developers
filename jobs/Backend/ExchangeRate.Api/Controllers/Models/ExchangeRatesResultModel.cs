using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Api.Controllers.Models
{
    public class ExchangeRatesResultModel
    {
        /// <summary>
        /// Returns the result exchage rates in a dictionary structure.
        /// </summary>
        /// <param name="Results"></param>
        public Dictionary<string, ExchangeRateProviderModel> Results { get; set; }
    }
}
