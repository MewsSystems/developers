CREATE VIEW [dbo].[vw_CurrencyPairAvailability]
AS
SELECT 
    bc.Code AS BaseCurrencyCode,
    tc.Code AS TargetCurrencyCode,
    COUNT(DISTINCT er.ProviderId) AS ProviderCount,
    -- STRING_AGG requires SQL Server 2017+
    -- For SQL Server 2016 and earlier, use FOR XML PATH instead
    STRING_AGG(p.Code, ', ') WITHIN GROUP (ORDER BY p.Code) AS Providers,
    MAX(er.ValidDate) AS LatestAvailableDate,
    COUNT(er.Id) AS TotalRecords
FROM [dbo].[ExchangeRate] er
INNER JOIN [dbo].[Currency] bc ON er.BaseCurrencyId = bc.Id
INNER JOIN [dbo].[Currency] tc ON er.TargetCurrencyId = tc.Id
INNER JOIN [dbo].[ExchangeRateProvider] p ON er.ProviderId = p.Id
WHERE p.IsActive = 1
GROUP BY bc.Code, tc.Code;
GO