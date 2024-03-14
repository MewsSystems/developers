namespace ExchangeRateService.ExternalServices;

internal record CNBDailyExratesResponse(CNBDailyExrate[] Rates);

internal record CNBDailyExrate(
    string ValidFor,
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    float Rate);