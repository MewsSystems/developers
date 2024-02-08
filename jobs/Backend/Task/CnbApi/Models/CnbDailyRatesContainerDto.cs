namespace CnbApi.Models;
//This data should be readonly as its the response from the API
public record CnbDailyRatesContainerDto(IReadOnlyList<CnbDailyRateDto> Rates);
public record CnbDailyRateDto(
        DateTime ValidFor,
        int Order,
        string Country,
        string Currency,
        int Amount,
        string CurrencyCode,
        decimal Rate
    );
