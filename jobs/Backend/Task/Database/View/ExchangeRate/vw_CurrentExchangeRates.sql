CREATE VIEW [dbo].[vw_CurrentExchangeRates]
AS
SELECT
    er.Id,
    p.Id AS ProviderId,
    p.Code AS ProviderCode,
    p.Name AS ProviderName,
    bc.Code AS BaseCurrencyCode,
    tc.Code AS TargetCurrencyCode,
    tc.Id AS TargetCurrencyId,
    er.Rate,
    er.Multiplier,
    CAST(er.Rate AS DECIMAL(19,6)) / NULLIF(er.Multiplier, 0) AS RatePerUnit,
    er.ValidDate,
    er.Created
FROM [dbo].[ExchangeRate] er
INNER JOIN [dbo].[Currency] bc ON er.BaseCurrencyId = bc.Id
INNER JOIN [dbo].[Currency] tc ON er.TargetCurrencyId = tc.Id
INNER JOIN [dbo].[ExchangeRateProvider] p ON er.ProviderId = p.Id
WHERE er.ValidDate = CAST(GETDATE() AS DATE)
  AND p.IsActive = 1;
GO