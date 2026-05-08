using ExchangeRateProvider.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Application.DTOs;

[ExcludeFromCodeCoverage(Justification = "DTO record with only auto-generated members - no custom logic to test")]
public record ExchangeRateDto(
    Currency BaseCurrency,
    Currency QuoteCurrency,
    decimal Rate
);
