using ExchangeRate.Api.Models;
using ExchangeRate.Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.Api.Controllers.Models
{
    public class ExchangeRateModel
    {
        /// <summary>
        /// The currency that is being converted from.
        /// </summary>
        [NotNull]
        public CurrencyModel? SourceCurrency { get; set; }

        /// <summary>
        /// The currency that is being converted to.
        /// </summary>
        [NotNull]
        public CurrencyModel? TargetCurrency { get; set; }

        [NotNull]
        public DateTime? Date {  get; set; }

    }
}