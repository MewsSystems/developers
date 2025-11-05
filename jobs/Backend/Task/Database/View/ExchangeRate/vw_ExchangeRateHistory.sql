CREATE VIEW [dbo].[vw_ExchangeRateHistory]
AS
WITH RateHistory AS (
    SELECT 
        er.Id,
        er.ProviderId,
        er.BaseCurrencyId,
        er.TargetCurrencyId,
        er.Rate,
        er.Multiplier,
        CAST(er.Rate AS DECIMAL(19,6)) / NULLIF(er.Multiplier, 0) AS RatePerUnit,
        er.ValidDate,
        er.Created,
        LAG(CAST(er.Rate AS DECIMAL(19,6)) / NULLIF(er.Multiplier, 0), 1) OVER (
            PARTITION BY er.ProviderId, er.BaseCurrencyId, er.TargetCurrencyId 
            ORDER BY er.ValidDate
        ) AS PreviousRatePerUnit
    FROM [dbo].[ExchangeRate] er
)
SELECT 
    rh.Id,
    p.Code AS ProviderCode,
    bc.Code AS BaseCurrencyCode,
    tc.Code AS TargetCurrencyCode,
    rh.Rate,
    rh.Multiplier,
    rh.RatePerUnit,
    rh.ValidDate,
    rh.PreviousRatePerUnit,
    
    -- Calculate rate change
    CASE 
        WHEN rh.PreviousRatePerUnit IS NOT NULL 
        THEN rh.RatePerUnit - rh.PreviousRatePerUnit
        ELSE NULL
    END AS RateChange,
    
    -- Calculate percentage change
    CASE 
        WHEN rh.PreviousRatePerUnit IS NOT NULL AND rh.PreviousRatePerUnit <> 0
        THEN ((rh.RatePerUnit - rh.PreviousRatePerUnit) / rh.PreviousRatePerUnit) * 100
        ELSE NULL
    END AS PercentageChange,
    
    rh.Created
FROM RateHistory rh
INNER JOIN [dbo].[Currency] bc ON rh.BaseCurrencyId = bc.Id
INNER JOIN [dbo].[Currency] tc ON rh.TargetCurrencyId = tc.Id
INNER JOIN [dbo].[ExchangeRateProvider] p ON rh.ProviderId = p.Id;
GO