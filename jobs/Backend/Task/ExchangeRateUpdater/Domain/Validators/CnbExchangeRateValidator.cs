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
        RuleFor(dto => dto.CurrencyCode)
            .NotEmpty().WithMessage(dto => $"Missing currency code for {dto.Country} currency");

        RuleFor(dto => dto.Amount)
            .GreaterThan(0).WithMessage(dto => $"Invalid amount for currency {dto.CurrencyCode}.");
        
        RuleFor(dto => dto.Rate)
            .GreaterThan(0).WithMessage(dto => $"Invalid rate for currency {dto.CurrencyCode}.");
        
        RuleFor(dto => dto.ValidFor)
            .NotEmpty().WithMessage(dto => $"Missing validFor date for currency {dto.CurrencyCode}.")
            .Must(BeValidDate).WithMessage(dto => $"Invalid date format for currency {dto.CurrencyCode}. Expected format: {DateFormat}.")
            .Must(BeNotInFuture).WithMessage(dto => $"The validFor date for currency {dto.CurrencyCode} is in the future.")
            .Must(BeNotOlderThanTolerance).WithMessage(dto => $"The validFor date for currency {dto.CurrencyCode} is older than {AllowedToleranceDays} days.");
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