namespace Mews.ExchangeRate.Provider.CNB.Dtos;
internal record CnbExRatesApiResponse(IEnumerable<ExchangeRate> Rates);