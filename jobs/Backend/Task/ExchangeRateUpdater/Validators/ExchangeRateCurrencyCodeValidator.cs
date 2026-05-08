using System;
using FluentValidation;

namespace ExchangeRateUpdater.Validators;

public class ExchangeRateCurrencyCodeValidator : AbstractValidator<string[]>
{
    public ExchangeRateCurrencyCodeValidator()
    {
        // Input Currency Codes validated to ISO Standard without Upper Casing for Ease of use.
        RuleFor(currencyCodes => currencyCodes)
            .NotEmpty()
            .WithName("Currency Codes");


        RuleForEach(currencyCodes => currencyCodes)
            .MaximumLength(3)
            .MinimumLength(3)
            .WithName("Currency Code"); 
    }
}