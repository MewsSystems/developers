CREATE PROCEDURE [dbo].[sp_BulkUpsertExchangeRates]
    @ProviderId INT,
    @ValidDate DATE,
    @RatesJson NVARCHAR(MAX) -- JSON: [{"currencyCode":"EUR","rate":24.335,"multiplier":1}]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON; -- Automatically rollback on error
    
    DECLARE @InsertedCount INT = 0;
    DECLARE @UpdatedCount INT = 0;
    DECLARE @SkippedCount INT = 0;
    DECLARE @BaseCurrencyId INT;
    
    BEGIN TRY
        -- Validate provider exists and get base currency
        SELECT @BaseCurrencyId = BaseCurrencyId 
        FROM [dbo].[ExchangeRateProvider] 
        WHERE Id = @ProviderId;
        
        IF @BaseCurrencyId IS NULL
            THROW 50100, 'Provider not found or has no base currency configured', 1;
        
        -- Validate JSON
        IF @RatesJson IS NULL OR @RatesJson = ''
            THROW 50101, 'Rates JSON cannot be empty', 1;
        
        IF ISJSON(@RatesJson) = 0
            THROW 50102, 'Invalid JSON format', 1;

        -- Check if validDate is within HistoricalDataDays range
        DECLARE @HistoricalDataDays INT;
        DECLARE @CutoffDate DATE;

        SELECT @HistoricalDataDays = TRY_CAST([Value] AS INT)
        FROM [dbo].[SystemConfiguration]
        WHERE [Key] = 'HistoricalDataDays';

        IF @HistoricalDataDays IS NULL
            SET @HistoricalDataDays = 90; -- Default value

        SET @CutoffDate = DATEADD(DAY, -@HistoricalDataDays, GETUTCDATE());

        IF @ValidDate < @CutoffDate
        BEGIN
            -- Return skipped result without processing
            SELECT
                0 AS InsertedCount,
                0 AS UpdatedCount,
                (SELECT COUNT(*) FROM OPENJSON(@RatesJson)) AS SkippedCount,
                0 AS ProcessedCount,
                (SELECT COUNT(*) FROM OPENJSON(@RatesJson)) AS TotalInJson,
                'SKIPPED - Date is outside ' + CAST(@HistoricalDataDays AS NVARCHAR(10)) + '-day range' AS [Status];
            RETURN;
        END

        BEGIN TRANSACTION;

        -- Track original JSON entry count for accurate reporting
        DECLARE @OriginalJsonCount INT = (SELECT COUNT(*) FROM OPENJSON(@RatesJson));

        -- Parse JSON into temp table with validation and deduplication
        -- If provider sends duplicate currency codes, keep the first one
        WITH ParsedRates AS (
            SELECT
                JSON_VALUE(value, '$.currencyCode') AS CurrencyCode,
                TRY_CAST(JSON_VALUE(value, '$.rate') AS DECIMAL(19,6)) AS Rate,
                TRY_CAST(JSON_VALUE(value, '$.multiplier') AS INT) AS Multiplier,
                ROW_NUMBER() OVER (PARTITION BY JSON_VALUE(value, '$.currencyCode') ORDER BY (SELECT NULL)) AS RowNum
            FROM OPENJSON(@RatesJson)
        )
        SELECT
            CurrencyCode,
            Rate,
            Multiplier
        INTO #TempRates
        FROM ParsedRates
        WHERE RowNum = 1; -- Keep first occurrence of each currency

        -- Validate parsed data
        IF EXISTS (SELECT 1 FROM #TempRates WHERE CurrencyCode IS NULL OR Rate IS NULL)
            THROW 50103, 'JSON contains invalid rate entries (missing currencyCode or rate)', 1;

        -- Set default multiplier if NULL
        UPDATE #TempRates SET Multiplier = 1 WHERE Multiplier IS NULL;
        
        -- Validate rates are positive
        IF EXISTS (SELECT 1 FROM #TempRates WHERE Rate <= 0)
            THROW 50104, 'All rates must be positive', 1;
        
        IF EXISTS (SELECT 1 FROM #TempRates WHERE Multiplier <= 0)
            THROW 50105, 'All multipliers must be positive', 1;

        -- Ensure all currencies exist (using MERGE for thread-safety)
        MERGE [dbo].[Currency] AS target
        USING (
            SELECT DISTINCT CurrencyCode
            FROM #TempRates
        ) AS source (Code)
        ON target.Code = source.Code
        WHEN NOT MATCHED BY TARGET THEN
            INSERT (Code)
            VALUES (source.Code);

        -- Merge operation with proper output capture
        DECLARE @MergeOutput TABLE (
            Action NVARCHAR(10),
            ExchangeRateId INT
        );
        
        MERGE [dbo].[ExchangeRate] AS target
        USING (
            SELECT 
                @ProviderId AS ProviderId,
                @BaseCurrencyId AS BaseCurrencyId,
                c.Id AS TargetCurrencyId,
                tr.Rate,
                tr.Multiplier,
                @ValidDate AS ValidDate
            FROM #TempRates tr
            INNER JOIN [dbo].[Currency] c ON tr.CurrencyCode = c.Code
            WHERE c.Id <> @BaseCurrencyId -- Don't create self-referencing rates
        ) AS source
        ON target.ProviderId = source.ProviderId
           AND target.BaseCurrencyId = source.BaseCurrencyId
           AND target.TargetCurrencyId = source.TargetCurrencyId
           AND target.ValidDate = source.ValidDate
        WHEN MATCHED THEN
            UPDATE SET 
                Rate = source.Rate,
                Multiplier = source.Multiplier,
                Modified = SYSDATETIMEOFFSET()
        WHEN NOT MATCHED BY TARGET THEN
            INSERT (ProviderId, BaseCurrencyId, TargetCurrencyId, Rate, Multiplier, ValidDate)
            VALUES (source.ProviderId, source.BaseCurrencyId, source.TargetCurrencyId, 
                    source.Rate, source.Multiplier, source.ValidDate)
        OUTPUT $action, INSERTED.Id
        INTO @MergeOutput;
        
        -- Calculate actual counts from merge output
        SELECT @InsertedCount = COUNT(*) FROM @MergeOutput WHERE Action = 'INSERT';
        SELECT @UpdatedCount = COUNT(*) FROM @MergeOutput WHERE Action = 'UPDATE';

        -- Calculate skipped count (duplicates in JSON + base currency + any filtered entries)
        SET @SkippedCount = @OriginalJsonCount - (@InsertedCount + @UpdatedCount);

        DROP TABLE #TempRates;

        COMMIT TRANSACTION;

        -- Return summary
        SELECT
            @InsertedCount AS InsertedCount,
            @UpdatedCount AS UpdatedCount,
            @SkippedCount AS SkippedCount,
            @InsertedCount + @UpdatedCount AS ProcessedCount,
            @OriginalJsonCount AS TotalInJson,
            'SUCCESS' AS Status;
            
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        IF OBJECT_ID('tempdb..#TempRates') IS NOT NULL
            DROP TABLE #TempRates;
        
        -- Return error details
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        DECLARE @ErrorLine INT = ERROR_LINE();
        
        -- Log error
        INSERT INTO [dbo].[ErrorLog] (Severity, Source, Message, Exception)
        VALUES (
            'Error',
            'usp_BulkUpsertExchangeRates',
            @ErrorMessage,
            CONCAT('Error ', @ErrorNumber, ' at line ', @ErrorLine)
        );
        
        -- Re-throw with context
        DECLARE @FullError NVARCHAR(4000) = 
            CONCAT('Bulk upsert failed: ', @ErrorMessage, 
                   ' (Error ', @ErrorNumber, ' at line ', @ErrorLine, ')');
        THROW 50199, @FullError, 1;
    END CATCH
END
GO