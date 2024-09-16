namespace Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates
{
    public class ResponseExchangeRates
    {
        public IEnumerable<ResponseExchangeRate> Rates { get; set; } = null!;
    }
}
