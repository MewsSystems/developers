using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Domain.Models.Dto
{
    [ExcludeFromCodeCoverage]
    public class ExchangeRatesResponseDto
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
    }
}
