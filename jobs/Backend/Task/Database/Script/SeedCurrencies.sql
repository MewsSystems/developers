-- =============================================
-- Insert CZK (Czech Koruna)
-- =============================================
PRINT '';
PRINT '1. Seeding CZK (Czech Koruna)...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Currency] WHERE Code = 'CZK')
BEGIN
    INSERT INTO [dbo].[Currency] (Code)
    VALUES ('CZK');
    PRINT '  ✓ CZK created (ID: ' + CAST(SCOPE_IDENTITY() AS NVARCHAR(10)) + ')';
END

-- =============================================
-- Insert EUR (Euro)
-- =============================================
PRINT '';
PRINT '2. Seeding EUR (Euro)...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Currency] WHERE Code = 'EUR')
BEGIN
    INSERT INTO [dbo].[Currency] (Code)
    VALUES ('EUR');
    PRINT '  ✓ EUR created (ID: ' + CAST(SCOPE_IDENTITY() AS NVARCHAR(10)) + ')';
END

-- =============================================
-- Insert RON (Romanian Leu)
-- =============================================
PRINT '';
PRINT '3. Seeding RON (Romanian Leu)...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Currency] WHERE Code = 'RON')
BEGIN
    INSERT INTO [dbo].[Currency] (Code)
    VALUES ('RON');
    PRINT '  ✓ RON created (ID: ' + CAST(SCOPE_IDENTITY() AS NVARCHAR(10)) + ')';
END