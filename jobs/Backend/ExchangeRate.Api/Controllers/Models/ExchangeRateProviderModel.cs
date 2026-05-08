using ExchangeRate.Api.Models;
using ExchangeRate.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ExchangeRate.Api.Controllers.Models
{
    /// <summary>
    /// Represents an exchange rate provider model containing the source and target currency along with the exchange value.
    /// </summary>
    public class ExchangeRateProviderModel
    {
        /// <summary>
        /// The currency that is being converted from.
        /// </summary>
        [JsonIgnore]
        public CurrencyModel? SourceCurrency { get; set; }

        /// <summary>
        /// The currency that is being converted to.
        /// </summary>
        [JsonIgnore]
        public CurrencyModel? TargetCurrency { get; set;}

        /// <summary>
        /// The amount to be converted.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The exchange rate value.
        /// </summary>
        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency?.Code}/{TargetCurrency?.Code} = {Value}";
        }
        public string Display => ToString();

    }
}