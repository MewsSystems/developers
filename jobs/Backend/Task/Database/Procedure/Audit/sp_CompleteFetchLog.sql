CREATE PROCEDURE [dbo].[sp_CompleteFetchLog]
    @LogId BIGINT,
    @Status NVARCHAR(20), -- 'Success', 'Failed', 'PartialSuccess'
    @RatesImported INT = NULL,
    @RatesUpdated INT = NULL,
    @ErrorMessage NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    DECLARE @ProviderId INT;
    DECLARE @CurrentStatus NVARCHAR(20);
    DECLARE @FetchCompleted DATETIMEOFFSET;
    
    BEGIN TRY
        -- Validate status value
        IF @Status NOT IN ('Success', 'Failed', 'PartialSuccess')
            THROW 50300, 'Invalid status. Must be Success, Failed, or PartialSuccess', 1;
        
        -- Get current log details
        SELECT 
            @ProviderId = ProviderId,
            @CurrentStatus = Status,
            @FetchCompleted = FetchCompleted
        FROM [dbo].[ExchangeRateFetchLog]
        WHERE Id = @LogId;
        
        -- Validate log exists
        IF @ProviderId IS NULL
            THROW 50301, 'Fetch log not found', 1;
        
        -- Prevent double completion
        IF @FetchCompleted IS NOT NULL
            THROW 50302, 'Fetch log already completed', 1;
        
        -- Validate that it was in Running state
        IF @CurrentStatus <> 'Running'
        BEGIN
            DECLARE @StateError NVARCHAR(200) = 
                CONCAT('Cannot complete fetch log with status ', @CurrentStatus, '. Expected Running.');
            THROW 50303, @StateError, 1;
        END
        
        BEGIN TRANSACTION;
        
        -- Update fetch log
        UPDATE [dbo].[ExchangeRateFetchLog]
        SET FetchCompleted = SYSDATETIMEOFFSET(),
            Status = @Status,
            RatesImported = @RatesImported,
            RatesUpdated = @RatesUpdated,
            ErrorMessage = @ErrorMessage
        WHERE Id = @LogId
          AND FetchCompleted IS NULL; -- Double-check with update lock
        
        IF @@ROWCOUNT = 0
            THROW 50304, 'Failed to update fetch log (possible race condition)', 1;
        
        -- Update provider health status based on result
        IF @Status = 'Success'
        BEGIN
            UPDATE [dbo].[ExchangeRateProvider]
            SET LastSuccessfulFetch = SYSDATETIMEOFFSET(),
                ConsecutiveFailures = 0,
                Modified = SYSDATETIMEOFFSET()
            WHERE Id = @ProviderId;
        END
        ELSE IF @Status = 'PartialSuccess'
        BEGIN
            -- Partial success: update last successful fetch but don't reset failure count
            UPDATE [dbo].[ExchangeRateProvider]
            SET LastSuccessfulFetch = SYSDATETIMEOFFSET(),
                Modified = SYSDATETIMEOFFSET()
            WHERE Id = @ProviderId;
        END
        ELSE IF @Status = 'Failed'
        BEGIN
            UPDATE [dbo].[ExchangeRateProvider]
            SET LastFailedFetch = SYSDATETIMEOFFSET(),
                ConsecutiveFailures = ConsecutiveFailures + 1,
                Modified = SYSDATETIMEOFFSET()
            WHERE Id = @ProviderId;
            
            -- Optionally auto-disable provider after too many failures
            DECLARE @ConsecutiveFailures INT;
            SELECT @ConsecutiveFailures = ConsecutiveFailures 
            FROM [dbo].[ExchangeRateProvider] 
            WHERE Id = @ProviderId;
            
            -- Auto-disable after 10 consecutive failures
            IF @ConsecutiveFailures >= 10
            BEGIN
                UPDATE [dbo].[ExchangeRateProvider]
                SET IsActive = 0,
                    Modified = SYSDATETIMEOFFSET()
                WHERE Id = @ProviderId;
                
                -- Log the auto-disable
                INSERT INTO [dbo].[ErrorLog] (Severity, Source, Message)
                VALUES (
                    'Critical',
                    'usp_CompleteFetchLog',
                    CONCAT('Provider ', @ProviderId, ' automatically disabled after ', 
                           @ConsecutiveFailures, ' consecutive failures')
                );
            END
        END
        
        COMMIT TRANSACTION;
        
        -- Return summary
        SELECT 
            @LogId AS LogId,
            @Status AS Status,
            @RatesImported AS RatesImported,
            @RatesUpdated AS RatesUpdated,
            @ProviderId AS ProviderId,
            CASE 
                WHEN @Status = 'Success' THEN 0
                WHEN @Status = 'Failed' THEN (
                    SELECT ConsecutiveFailures 
                    FROM [dbo].[ExchangeRateProvider] 
                    WHERE Id = @ProviderId
                )
                ELSE NULL
            END AS ProviderConsecutiveFailures,
            'SUCCESS' AS CompletionStatus;
            
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage2 NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        
        -- Log error
        INSERT INTO [dbo].[ErrorLog] (Severity, Source, Message, Exception)
        VALUES (
            'Error',
            'usp_CompleteFetchLog',
            @ErrorMessage2,
            CONCAT('Error ', @ErrorNumber, ' for LogId: ', @LogId)
        );
        
        THROW;
    END CATCH
END
GO