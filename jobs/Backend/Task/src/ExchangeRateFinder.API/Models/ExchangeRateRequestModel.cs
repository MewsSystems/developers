using System.ComponentModel.DataAnnotations;

namespace ExchangeRateFinder.API.RequestModels
{
    public class ExchangeRateRequestModel
    {
        [Required(ErrorMessage = "Source currency code is required.")]
        public string SourceCurrencyCode { get; init; } = string.Empty;

        [Required(ErrorMessage = "Target currency codes are required.")]
        public string TargetCurrencyCodes { get; init; } = string.Empty;
    }
}
