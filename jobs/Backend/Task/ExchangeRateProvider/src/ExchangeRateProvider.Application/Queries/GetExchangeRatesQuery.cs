using ExchangeRateProvider.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Application.Queries;

[ExcludeFromCodeCoverage(Justification = "Query record with only auto-generated members - no custom logic to test")]
public record GetExchangeRatesQuery(
    Currency BaseCurrency,
    List<Currency>? QuoteCurrencies = null
);
