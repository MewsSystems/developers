CREATE PROCEDURE [dbo].[sp_StartFetchLog]
    @ProviderId INT,
    @RequestedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @LogId BIGINT;
    
    BEGIN TRY
        -- Validate provider exists
        IF NOT EXISTS (SELECT 1 FROM [dbo].[ExchangeRateProvider] WHERE Id = @ProviderId)
            THROW 50200, 'Provider not found', 1;
        
        -- Validate user exists (if provided)
        IF @RequestedBy IS NOT NULL 
           AND NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE Id = @RequestedBy)
            THROW 50201, 'User not found', 1;
        
        -- Insert fetch log
        INSERT INTO [dbo].[ExchangeRateFetchLog] (ProviderId, RequestedBy, Status)
        VALUES (@ProviderId, @RequestedBy, 'Running');
        
        SET @LogId = SCOPE_IDENTITY();
        
        -- Return log ID and timestamp
        SELECT 
            @LogId AS LogId,
            SYSDATETIMEOFFSET() AS FetchStarted,
            'SUCCESS' AS Status;
            
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        
        -- Log error
        INSERT INTO [dbo].[ErrorLog] (Severity, Source, Message)
        VALUES ('Error', 'usp_StartFetchLog', @ErrorMessage);
        
        THROW;
    END CATCH
END
GO