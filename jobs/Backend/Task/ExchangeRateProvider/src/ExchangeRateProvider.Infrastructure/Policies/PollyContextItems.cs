using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Infrastructure.Policies;

[ExcludeFromCodeCoverage(Justification = "Contains only constant string declarations - no testable logic")]
public static class PolicyContextItems
{
    public const string Logger = "logger";
}
