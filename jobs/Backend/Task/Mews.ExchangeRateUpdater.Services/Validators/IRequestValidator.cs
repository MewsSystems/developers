using Mews.ExchangeRateUpdater.Dtos;

namespace Mews.ExchangeRateUpdater.Services.Validators
{
    public interface IRequestValidator
    {
        public List<string> Validate(ref List<CurrencyDto> currencies, ref List<string> validationMessages);
    }
}
