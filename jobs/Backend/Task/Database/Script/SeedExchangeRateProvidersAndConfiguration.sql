-- =============================================
-- Get Currency IDs
-- =============================================
DECLARE @CzkId INT, @EurId INT, @RonId INT;

SELECT @CzkId = Id FROM [dbo].[Currency] WHERE Code = 'CZK';
SELECT @EurId = Id FROM [dbo].[Currency] WHERE Code = 'EUR';
SELECT @RonId = Id FROM [dbo].[Currency] WHERE Code = 'RON';

-- Ensure currencies exist
IF @CzkId IS NULL OR @EurId IS NULL OR @RonId IS NULL
BEGIN
    PRINT 'ERROR: Required currencies (CZK, EUR, RON) not found!';
    PRINT 'Please run the currency seed script first.';
    RETURN;
END

-- =============================================
-- 1. CNB (Czech National Bank) - XML FORMAT
-- =============================================
PRINT '';
PRINT '1. Seeding CNB (Czech National Bank)...';

DECLARE @CnbId INT;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProvider] WHERE Code = 'CNB')
BEGIN
    INSERT INTO [dbo].[ExchangeRateProvider] 
    (
        Name, 
        Code, 
        Url, 
        BaseCurrencyId, 
        RequiresAuthentication, 
        IsActive
    )
    VALUES 
    (
        'Czech National Bank',
        'CNB',
        'https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml',
        @CzkId,
        0,
        1
    );
    
    SET @CnbId = SCOPE_IDENTITY();
    PRINT '  ✓ CNB provider created (ID: ' + CAST(@CnbId AS NVARCHAR(10)) + ')';
END
ELSE
BEGIN
    SELECT @CnbId = Id FROM [dbo].[ExchangeRateProvider] WHERE Code = 'CNB';
    PRINT '  ⚠ CNB provider already exists (ID: ' + CAST(@CnbId AS NVARCHAR(10)) + ')';
END

-- CNB Configurations
IF @CnbId IS NOT NULL
BEGIN
    -- Format (XML)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'Format')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'Format', 'XML', 'Response format (XML with custom schema)');
    
    -- Decimal Separator
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'DecimalSeparator')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'DecimalSeparator', 'Comma', 'Uses comma (,) as decimal separator');
    
    -- Update Time
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'UpdateTime')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'UpdateTime', '14:30', 'Daily update time (after 14:30 CET)');
    
    -- Time Zone
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'TimeZone')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'TimeZone', 'CET', 'Central European Time');
    
    -- Encoding
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'Encoding')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'Encoding', 'UTF-8', 'Character encoding');
    
    -- XML Namespace
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'XmlNamespace')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'XmlNamespace', 'http://www.cnb.cz/xsd/Filharmonie/modely/Denni_kurz/1.1', 'CNB XML namespace');
    
    -- Has Variable Amounts
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'HasVariableAmounts')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'HasVariableAmounts', 'true', 'Uses variable amounts (mnozstvi: 1, 100, 1000)');
    
    -- XML Structure
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'XmlStructure')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'XmlStructure', 'kurzy/tabulka/radek', 'Uses kurzy > tabulka > radek structure');
    
    -- Root Element
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'RootElement')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'RootElement', 'kurzy', 'Root element with datum and poradi attributes');
    
    -- Has Date Attribute
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'HasDateAttribute')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'HasDateAttribute', 'true', 'Date in root element as datum attribute (DD.MM.YYYY)');
    
    -- Has Sequence Number
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'HasSequenceNumber')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'HasSequenceNumber', 'true', 'Sequence number in root element as poradi attribute');
    
    -- Historical URL
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @CnbId AND SettingKey = 'HistoricalUrl')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@CnbId, 'HistoricalUrl', 'https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml?date={date}', 'Historical rates URL (use DD.MM.YYYY format)');
    
    PRINT '  ✓ CNB configurations added';
END

-- =============================================
-- 2. ECB (European Central Bank)
-- =============================================
PRINT '';
PRINT '2. Seeding ECB (European Central Bank)...';

DECLARE @EcbId INT;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProvider] WHERE Code = 'ECB')
BEGIN
    INSERT INTO [dbo].[ExchangeRateProvider] 
    (
        Name, 
        Code, 
        Url, 
        BaseCurrencyId, 
        RequiresAuthentication, 
        IsActive
    )
    VALUES 
    (
        'European Central Bank',
        'ECB',
        'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml',
        @EurId,
        0,
        1
    );
    
    SET @EcbId = SCOPE_IDENTITY();
    PRINT '  ✓ ECB provider created (ID: ' + CAST(@EcbId AS NVARCHAR(10)) + ')';
END
ELSE
BEGIN
    SELECT @EcbId = Id FROM [dbo].[ExchangeRateProvider] WHERE Code = 'ECB';
    PRINT '  ⚠ ECB provider already exists (ID: ' + CAST(@EcbId AS NVARCHAR(10)) + ')';
END

-- ECB Configurations
IF @EcbId IS NOT NULL
BEGIN
    -- Format
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'Format')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'Format', 'XML', 'Response format (XML with gesmes namespace)');
    
    -- Decimal Separator
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'DecimalSeparator')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'DecimalSeparator', 'Dot', 'Uses dot (.) as decimal separator');
    
    -- Update Time
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'UpdateTime')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'UpdateTime', '16:00', 'Daily update time (around 16:00 CET)');
    
    -- Time Zone
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'TimeZone')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'TimeZone', 'CET', 'Central European Time');
    
    -- Namespace
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'XmlNamespace')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'XmlNamespace', 'http://www.gesmes.org/xml/2002-08-01', 'gesmes XML namespace');
    
    -- Has Variable Amounts
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'HasVariableAmounts')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'HasVariableAmounts', 'false', 'Always uses 1 unit');
    
    -- Nesting Structure
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'XmlStructure')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'XmlStructure', 'TripleNestedCube', 'Uses Envelope > Cube > Cube > Cube structure');
    
    -- Historical URL (90 days)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'Historical90DaysUrl')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'Historical90DaysUrl', 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml', 'Last 90 days historical rates');
    
    -- Historical URL (all)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @EcbId AND SettingKey = 'HistoricalAllUrl')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@EcbId, 'HistoricalAllUrl', 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml', 'All historical rates (large file)');
    
    PRINT '  ✓ ECB configurations added';
END

-- =============================================
-- 3. BNR (Romanian National Bank)
-- =============================================
PRINT '';
PRINT '3. Seeding BNR (Banca Națională a României)...';

DECLARE @BnrId INT;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProvider] WHERE Code = 'BNR')
BEGIN
    INSERT INTO [dbo].[ExchangeRateProvider] 
    (
        Name, 
        Code, 
        Url, 
        BaseCurrencyId, 
        RequiresAuthentication, 
        IsActive
    )
    VALUES 
    (
        'Banca Națională a României',
        'BNR',
        'https://www.bnr.ro/nbrfxrates.xml',
        @RonId,
        0,
        1
    );
    
    SET @BnrId = SCOPE_IDENTITY();
    PRINT '  ✓ BNR provider created (ID: ' + CAST(@BnrId AS NVARCHAR(10)) + ')';
END
ELSE
BEGIN
    SELECT @BnrId = Id FROM [dbo].[ExchangeRateProvider] WHERE Code = 'BNR';
    PRINT '  ⚠ BNR provider already exists (ID: ' + CAST(@BnrId AS NVARCHAR(10)) + ')';
END

-- BNR Configurations
IF @BnrId IS NOT NULL
BEGIN
    -- Format
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'Format')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'Format', 'XML', 'Response format (XML with custom schema)');
    
    -- Decimal Separator
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'DecimalSeparator')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'DecimalSeparator', 'Dot', 'Uses dot (.) as decimal separator');
    
    -- Update Time
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'UpdateTime')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'UpdateTime', '13:00', 'Daily update time (13:00 EET)');
    
    -- Time Zone
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'TimeZone')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'TimeZone', 'EET', 'Eastern European Time');
    
    -- Namespace
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'XmlNamespace')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'XmlNamespace', 'http://www.bnr.ro/xsd', 'BNR XML namespace');
    
    -- Has Variable Amounts
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'HasVariableAmounts')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'HasVariableAmounts', 'true', 'Uses multiplier attribute (100, 1000)');
    
    -- XML Structure
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'XmlStructure')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'XmlStructure', 'DataSet/Body/Cube/Rate', 'Uses DataSet > Header/Body > Cube > Rate structure');
    
    -- Has Header
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'HasHeader')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'HasHeader', 'true', 'Has separate Header and Body sections');
    
    -- Historical URL Template
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProviderConfiguration] WHERE ProviderId = @BnrId AND SettingKey = 'HistoricalUrl')
        INSERT INTO [dbo].[ExchangeRateProviderConfiguration] (ProviderId, SettingKey, SettingValue, Description)
        VALUES (@BnrId, 'HistoricalUrl', 'https://www.bnr.ro/files/xml/years/nbrfxrates{year}.xml', 'Historical rates by year');
    
    PRINT '  ✓ BNR configurations added';
END