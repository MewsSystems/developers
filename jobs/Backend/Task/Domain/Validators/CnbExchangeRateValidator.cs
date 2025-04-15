using System;
using System.Globalization;
using ExchangeRateUpdater.Domain.DTOs;
using FluentValidation;

namespace ExchangeRateUpdater.Domain.Validators;

public class CnbExchangeRateValidator : AbstractValidator<CnbExchangeRateDto>
{
    private const int AllowedToleranceDays = 5;
    private const string DateFormat = "yyyy-MM-dd";
    
    public CnbExchangeRateValidator()
    {
        RuleFor(cnbExchangeRateDto => cnbExchangeRateDto.CurrencyCode)
            .NotEmpty().WithMessage(entry => $"Missing currency code for {entry.Country} currency");

        RuleFor(cnbExchangeRateDto => cnbExchangeRateDto.Amount)
            .GreaterThan(0).WithMessage(cnbExchangeRateDto => $"Invalid amount for currency {cnbExchangeRateDto.CurrencyCode}.");
        
        RuleFor(cnbExchangeRateDto => cnbExchangeRateDto.Rate)
            .GreaterThan(0).WithMessage(cnbExchangeRateDto => $"Invalid rate for currency {cnbExchangeRateDto.CurrencyCode}.");
        
        RuleFor(entry => entry.ValidFor)
            .NotEmpty().WithMessage(entry => $"Missing validFor date for currency {entry.CurrencyCode}.")
            .Must(BeValidDate).WithMessage(x => $"Invalid date format for currency {x.CurrencyCode}. Expected format: {DateFormat}.")
            .Must(BeNotInFuture).WithMessage(x => $"The validFor date for currency {x.CurrencyCode} is in the future.")
            .Must(BeNotOlderThanTolerance).WithMessage(x => $"The validFor date for currency {x.CurrencyCode} is older than {AllowedToleranceDays} days.");
    }
    
    private bool BeValidDate(string validFor)
    {
        return DateTime.TryParseExact(validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
    
    private bool BeNotInFuture(string validFor)
    {
        if (!DateTime.TryParseExact(validFor, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            return true;

        return parsed.Date <= DateTime.Today;
    }

    private bool BeNotOlderThanTolerance(string validFor)
    {
        if (!DateTime.TryParseExact(validFor, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            return true; 

        var diff = (DateTime.Today - parsed.Date).TotalDays;
        return diff <= AllowedToleranceDays;
    }
}