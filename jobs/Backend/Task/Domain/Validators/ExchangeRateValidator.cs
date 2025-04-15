using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Domain.DTOs;

namespace ExchangeRateUpdater.Domain.Validators;

public interface IExchangeRateValidator
{
    IEnumerable<string> Validate(RateDto rateDto);
}
public class ExchangeRateValidator : IExchangeRateValidator
{
    public IEnumerable<string> Validate(RateDto rateDto)
    {
        var errors = new List<string>();

        if (rateDto == null)
        {
            errors.Add("Response data is null.");
            return errors;
        }
        
        if (rateDto.ExchangeRateDtos == null || !rateDto.ExchangeRateDtos.Any())
        {
            errors.Add("No exchange rate data found.");
            return errors;
        }
        
        foreach (var dto in rateDto.ExchangeRateDtos)
        {
            if (string.IsNullOrWhiteSpace(dto.CurrencyCode))
                errors.Add("Missing currency code.");

            if (dto.Amount <= 0)
                errors.Add($"Invalid amount for currency {dto.CurrencyCode}.");

            if (dto.Rate <= 0)
                errors.Add($"Invalid rate for currency {dto.CurrencyCode}.");

            if (!DateTime.TryParse(dto.ValidFor, out var validForDate))
                errors.Add($"Invalid date format for currency {dto.CurrencyCode}.");
            else
            {
                if (validForDate.Date != DateTime.Today)
                    errors.Add($"The validFor date for currency {dto.CurrencyCode} is outdated ({dto.ValidFor}). Expected date: {DateTime.Today:yyyy-MM-dd}.");
            }
            
            if (string.IsNullOrWhiteSpace(dto.Country))
                errors.Add($"Missing country information for currency {dto.CurrencyCode}.");
        }

        return errors;
    }
}