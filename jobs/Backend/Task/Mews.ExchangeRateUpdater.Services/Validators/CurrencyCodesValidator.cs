using ISO._4217;
using Mews.ExchangeRateUpdater.Dtos;

namespace Mews.ExchangeRateUpdater.Services.Validators
{
    /// <summary>
    /// This is an implementation to validate the currency code collection input
    /// </summary>
    public class CurrencyCodesValidator : IRequestValidator
    {
        public List<string> Validate(ref List<CurrencyDto> currencies, ref List<string> validationMessages)
        {
            if (ValidateCurrencyCodesRequiredCriteria(ref currencies, ref validationMessages)) return validationMessages;

            if (ValidateCurrencyCodesEmptyCriteria(ref currencies, ref validationMessages)) return validationMessages;

            currencies = currencies.DistinctBy(c => c.Code, StringComparer.OrdinalIgnoreCase).ToList();

            ValidateAndSanitiseCurrencyCodesValidCriteria(ref currencies, ref validationMessages);

            LogValidationMessagesToConsoleIfExists(validationMessages);

            return validationMessages;
        }

        private bool ValidateCurrencyCodesRequiredCriteria(ref List<CurrencyDto> currencies, ref List<string> validationMessages)
        {
            var hasHitTheCriteria = false;

            if (currencies == null)
            {
                validationMessages.Add("Currency codes collection cannot be null or empty");
                hasHitTheCriteria = true;
            }

            return hasHitTheCriteria;
        }

        private bool ValidateCurrencyCodesEmptyCriteria(ref List<CurrencyDto> currencies, ref List<string> validationMessages)
        {
            var hasHitTheCriteria = false;

            if (currencies != null && !currencies.Any())
            {
                validationMessages.Add("Currency codes collection cannot be null or empty");
                hasHitTheCriteria = true;
            }

            return hasHitTheCriteria;
        }

        private void ValidateAndSanitiseCurrencyCodesValidCriteria(ref List<CurrencyDto> currencies, ref List<string> validationMessages)
        {
            var isoCurrencyCodes = CurrencyCodesResolver.Codes;

            var invalidCurrencies = currencies.Where(currency => !isoCurrencyCodes.Any(isoCurrencyCode => isoCurrencyCode.Code == currency.Code.ToUpperInvariant()));

            if (invalidCurrencies.Count() > 0)
            {
                validationMessages.Add($"There are some invalid currency codes present in the input, removing those codes before fetching the exchange rates : [{string.Join(", ", invalidCurrencies)}]");
                currencies = currencies.Where(currency =>  !invalidCurrencies.Any(invalidCurrency => invalidCurrency.Code.ToUpperInvariant() == currency.Code.ToUpperInvariant())).ToList();
            }
        }

        private void LogValidationMessagesToConsoleIfExists(List<string> validationMessages)
        {
            Console.WriteLine(string.Join("\n", validationMessages));
        }
    }
}
