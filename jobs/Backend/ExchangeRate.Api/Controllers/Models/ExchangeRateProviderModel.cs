using ExchangeRate.Api.Models;
using ExchangeRate.Application.DTOs;
using System.ComponentModel.DataAnnotations;

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
        public CurrencyModel? SourceCurrency { get; }

        /// <summary>
        /// The currency that is being converted to.
        /// </summary>
        public CurrencyModel? TargetCurrency { get; }

        /// <summary>
        /// The amount to be converted.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The exchange rate value.
        /// </summary>
        public decimal Value { get; set; }
    }
}