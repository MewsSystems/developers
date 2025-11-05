CREATE VIEW [dbo].[vw_ProviderConfigurationSummary]
AS
SELECT 
    p.Id AS ProviderId,
    p.Code AS ProviderCode,
    p.Name AS ProviderName,
    p.Url,
    bc.Code AS BaseCurrencyCode,
    p.IsActive,
    
    -- Pivot common settings
    MAX(CASE WHEN pc.SettingKey = 'Format' THEN pc.SettingValue END) AS Format,
    MAX(CASE WHEN pc.SettingKey = 'DecimalSeparator' THEN pc.SettingValue END) AS DecimalSeparator,
    MAX(CASE WHEN pc.SettingKey = 'UpdateTime' THEN pc.SettingValue END) AS UpdateTime,
    MAX(CASE WHEN pc.SettingKey = 'TimeZone' THEN pc.SettingValue END) AS TimeZone,
    
    -- Count of settings
    COUNT(pc.Id) AS TotalSettings,
    
    p.Created,
    p.Modified
FROM [dbo].[ExchangeRateProvider] p
INNER JOIN [dbo].[Currency] bc ON p.BaseCurrencyId = bc.Id
LEFT JOIN [dbo].[ExchangeRateProviderConfiguration] pc ON p.Id = pc.ProviderId
GROUP BY 
    p.Id, p.Code, p.Name, p.Url, bc.Code, p.IsActive, p.Created, p.Modified;
GO