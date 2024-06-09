using Application.Common.Models;
using Application.Common.Validations;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services
{
    public class CurrenciesValidationService(
        CurrencyValidator _currencyValidator, 
        ILogger<CurrenciesValidationService> _logger) : ICurrenciesValidationService
    {
        // this only logs a warning as console app has an invalid input but expects results
        // if this application grows more complicated might want to consider cqrs / mediatr pattern with automatic validation on commands
        public void ValidateAndLogWarning(IEnumerable<Currency> currencies)
        {
            var validationResults = currencies.Select(c => _currencyValidator.Validate(c));
            if (validationResults.Any(vr => !vr.IsValid))
            {
                var errorSummary = string.Join(", ", validationResults.SelectMany(vr => vr.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning($"Validation Error on Currency inputs: {errorSummary}");
            }
        }
    }
}
