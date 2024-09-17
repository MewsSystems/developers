namespace Mews.ExchangeRate.Provider.CNB.Dtos;
internal record ExchangeRate(
    int Amount,
    string Country,
    string Currency,
    string CurrencyCode,
    int Order,
    decimal Rate,
    string ValidFor
);
