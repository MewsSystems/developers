using Dapper;
using DataLayer.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text.Json;

namespace DataLayer.Dapper;

public class StoredProcedureService : IStoredProcedureService
{
    private readonly IDapperContext _dapperContext;
    private readonly bool _useInMemoryDatabase;

    public StoredProcedureService(IDapperContext dapperContext, IConfiguration configuration)
    {
        _dapperContext = dapperContext;
        _useInMemoryDatabase = configuration.GetValue<bool>("Database:UseInMemoryDatabase");
    }

    public async Task<BulkUpsertResult> BulkUpsertExchangeRatesAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<ExchangeRateInput> rates,
        CancellationToken cancellationToken = default)
    {
        if (_useInMemoryDatabase)
        {
            return await BulkUpsertExchangeRatesSqliteAsync(providerId, validDate, rates, cancellationToken);
        }
        else
        {
            return await BulkUpsertExchangeRatesSqlServerAsync(providerId, validDate, rates, cancellationToken);
        }
    }

    private async Task<BulkUpsertResult> BulkUpsertExchangeRatesSqlServerAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<ExchangeRateInput> rates,
        CancellationToken cancellationToken)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        // Use camelCase for JSON property names to match stored procedure expectations
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var ratesJson = JsonSerializer.Serialize(rates, jsonOptions);

        var parameters = new DynamicParameters();
        parameters.Add("@ProviderId", providerId);
        parameters.Add("@ValidDate", validDate.ToDateTime(TimeOnly.MinValue));
        parameters.Add("@RatesJson", ratesJson);

        var result = await connection.QuerySingleAsync<BulkUpsertResult>(
            "[dbo].[sp_BulkUpsertExchangeRates]",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    private async Task<BulkUpsertResult> BulkUpsertExchangeRatesSqliteAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<ExchangeRateInput> rates,
        CancellationToken cancellationToken)
    {
        // Retry logic for SQLite concurrency issues
        const int maxRetries = 5;
        var random = new Random();

        for (int retry = 0; retry <= maxRetries; retry++)
        {
            var result = new BulkUpsertResult
            {
                InsertedCount = 0,
                TotalInJson = 0,
                ProcessedCount = 0,
                SkippedCount = 0,
                UpdatedCount = 0,
                Status = ""
            };
            var chunks = rates.Chunk(16);
            var index = 0;
            Debug.WriteLine($"Started processing for provider {providerId}.");
            LETS_TRY_AGAIN:
            for (var i = index; i < chunks.Count(); i++)
            {
                try
                {
                    await Task.Delay(random.Next(0, 1000));
                    Debug.WriteLine($"Processing chunk {i} of {chunks.Count()} for provider {providerId}.");
                    var chunkResult = await ExecuteBulkUpsertWithTransactionAsync(providerId, validDate, chunks.ElementAt(i), cancellationToken);
                    result.InsertedCount += chunkResult.InsertedCount;
                    result.TotalInJson += chunkResult.TotalInJson;
                    result.ProcessedCount += chunkResult.ProcessedCount;
                    result.SkippedCount += chunkResult.SkippedCount;
                    result.UpdatedCount += chunkResult.UpdatedCount;
                    result.Status = chunkResult.Status;
                    Debug.WriteLine($"Completed processing chunk {i} of {chunks.Count()} for provider {providerId}.");
                    await Task.Delay(random.Next(0, 1000));
                }
                catch (Microsoft.Data.Sqlite.SqliteException ex) when (
                        (ex.SqliteErrorCode == 5 || ex.SqliteErrorCode == 6) && // SQLITE_BUSY or SQLITE_LOCKED
                        retry < maxRetries)
                {
                    Debug.WriteLine($"Failed processing chunk {i} of {chunks.Count()} for provider {providerId}.");
                    goto LETS_TRY_AGAIN;
                }
            }
            Debug.WriteLine($"Completed processing for provider {providerId}.");
            return result;
        }

        throw new InvalidOperationException("Bulk upsert failed after multiple retries due to database locking");
    }

    private async Task<BulkUpsertResult> ExecuteBulkUpsertWithTransactionAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<ExchangeRateInput> rates,
        CancellationToken cancellationToken)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        try
        {
            var ratesList = rates.ToList();

            // Validate input
            if (!ratesList.Any())
                throw new InvalidOperationException("Rates cannot be empty");

            // Validate all rates and multipliers are positive
            if (ratesList.Any(r => r.Rate <= 0))
                throw new InvalidOperationException("All rates must be positive");

            if (ratesList.Any(r => r.Multiplier <= 0))
                throw new InvalidOperationException("All multipliers must be positive");

            // Check if validDate is within HistoricalDataDays range
            var historicalDataDays = await connection.ExecuteScalarAsync<int?>(
                "SELECT CAST(Value AS INTEGER) FROM SystemConfiguration WHERE Key = 'HistoricalDataDays'",
                transaction: transaction) ?? 90;

            var cutoffDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-historicalDataDays));

            if (validDate < cutoffDate)
            {
                // Skip rates outside the historical data range
                return new BulkUpsertResult
                {
                    InsertedCount = 0,
                    UpdatedCount = 0,
                    SkippedCount = ratesList.Count,
                    ProcessedCount = 0,
                    TotalInJson = ratesList.Count,
                    Status = $"SKIPPED - Date {validDate} is outside {historicalDataDays}-day range (cutoff: {cutoffDate})"
                };
            }

            // 1. Validate provider exists and get base currency
            var baseCurrencyId = await connection.ExecuteScalarAsync<int?>(
                "SELECT BaseCurrencyId FROM ExchangeRateProvider WHERE Id = @ProviderId",
                new { ProviderId = providerId },
                transaction);

            if (baseCurrencyId == null)
                throw new InvalidOperationException($"Provider {providerId} not found or has no base currency configured");

            // 2. Ensure all currencies exist
            foreach (var rate in ratesList)
            {
                var currencyExists = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM Currency WHERE Code = @Code",
                    new { Code = rate.CurrencyCode },
                    transaction);

                if (currencyExists == 0)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO Currency (Code) VALUES (@Code)",
                        new { Code = rate.CurrencyCode },
                        transaction);
                }
            }

            // 3. Process each rate (upsert)
            int insertedCount = 0;
            int updatedCount = 0;
            int skippedCount = 0;
            var validDateString = validDate.ToString("yyyy-MM-dd");

            foreach (var rate in ratesList)
            {
                var targetCurrencyId = await connection.ExecuteScalarAsync<int>(
                    "SELECT Id FROM Currency WHERE Code = @Code",
                    new { Code = rate.CurrencyCode },
                    transaction);

                // Skip self-referencing rates
                if (targetCurrencyId == baseCurrencyId)
                {
                    skippedCount++;
                    continue;
                }

                var existingRateId = await connection.ExecuteScalarAsync<int?>(
                    @"SELECT Id FROM ExchangeRate
                      WHERE ProviderId = @ProviderId
                        AND BaseCurrencyId = @BaseCurrencyId
                        AND TargetCurrencyId = @TargetCurrencyId
                        AND ValidDate = @ValidDate",
                    new
                    {
                        ProviderId = providerId,
                        BaseCurrencyId = baseCurrencyId.Value,
                        TargetCurrencyId = targetCurrencyId,
                        ValidDate = validDateString
                    },
                    transaction);

                if (existingRateId.HasValue)
                {
                    // Update existing rate
                    await connection.ExecuteAsync(
                        @"UPDATE ExchangeRate
                          SET Rate = @Rate,
                              Multiplier = @Multiplier,
                              Modified = datetime('now')
                          WHERE Id = @Id",
                        new
                        {
                            Id = existingRateId.Value,
                            Rate = rate.Rate,
                            Multiplier = rate.Multiplier
                        },
                        transaction);
                    updatedCount++;
                }
                else
                {
                    // Insert new rate
                    await connection.ExecuteAsync(
                        @"INSERT INTO ExchangeRate
                          (ProviderId, BaseCurrencyId, TargetCurrencyId, Rate, Multiplier, ValidDate, Created)
                          VALUES (@ProviderId, @BaseCurrencyId, @TargetCurrencyId, @Rate, @Multiplier, @ValidDate, datetime('now'))",
                        new
                        {
                            ProviderId = providerId,
                            BaseCurrencyId = baseCurrencyId.Value,
                            TargetCurrencyId = targetCurrencyId,
                            Rate = rate.Rate,
                            Multiplier = rate.Multiplier,
                            ValidDate = validDateString
                        },
                        transaction);
                    insertedCount++;
                }
            }

            transaction.Commit();

            return new BulkUpsertResult
            {
                InsertedCount = insertedCount,
                UpdatedCount = updatedCount,
                SkippedCount = skippedCount,
                ProcessedCount = insertedCount + updatedCount,
                TotalInJson = ratesList.Count,
                Status = "SUCCESS"
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new InvalidOperationException($"Bulk upsert failed: {ex.Message}", ex);
        }
    }

    public async Task<StartFetchLogResult> StartFetchLogAsync(
        int providerId,
        int? requestedBy = null,
        CancellationToken cancellationToken = default)
    {
        if (_useInMemoryDatabase)
        {
            return await StartFetchLogSqliteAsync(providerId, requestedBy, cancellationToken);
        }
        else
        {
            return await StartFetchLogSqlServerAsync(providerId, requestedBy, cancellationToken);
        }
    }

    private async Task<StartFetchLogResult> StartFetchLogSqlServerAsync(
        int providerId,
        int? requestedBy,
        CancellationToken cancellationToken)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@ProviderId", providerId);
        parameters.Add("@RequestedBy", requestedBy);

        var result = await connection.QuerySingleAsync<StartFetchLogResult>(
            "[dbo].[sp_StartFetchLog]",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    private async Task<StartFetchLogResult> StartFetchLogSqliteAsync(
        int providerId,
        int? requestedBy,
        CancellationToken cancellationToken)
    {
        // Add small random delay to reduce contention (0-50ms)
        await Task.Delay(new Random().Next(0, 50), cancellationToken);

        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        // Validate provider exists
        var providerExists = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM ExchangeRateProvider WHERE Id = @ProviderId",
            new { ProviderId = providerId });

        if (providerExists == 0)
            throw new InvalidOperationException("Provider not found");

        // Validate user exists (if provided)
        if (requestedBy.HasValue)
        {
            var userExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM User WHERE Id = @UserId",
                new { UserId = requestedBy.Value });

            if (userExists == 0)
                throw new InvalidOperationException("User not found");
        }

        // Insert fetch log
        var logId = await connection.ExecuteScalarAsync<long>(
            @"INSERT INTO ExchangeRateFetchLog (ProviderId, RequestedBy, Status, FetchStarted)
              VALUES (@ProviderId, @RequestedBy, 'Running', datetime('now'));
              SELECT last_insert_rowid();",
            new
            {
                ProviderId = providerId,
                RequestedBy = requestedBy
            });

        return new StartFetchLogResult
        {
            LogId = logId,
            FetchStarted = DateTimeOffset.UtcNow,
            Status = "SUCCESS"
        };
    }

    public async Task<CompleteFetchLogResult> CompleteFetchLogAsync(
        long logId,
        string status,
        int? ratesImported = null,
        int? ratesUpdated = null,
        string? errorMessage = null,
        CancellationToken cancellationToken = default)
    {
        if (_useInMemoryDatabase)
        {
            return await CompleteFetchLogSqliteAsync(logId, status, ratesImported, ratesUpdated, errorMessage, cancellationToken);
        }
        else
        {
            return await CompleteFetchLogSqlServerAsync(logId, status, ratesImported, ratesUpdated, errorMessage, cancellationToken);
        }
    }

    private async Task<CompleteFetchLogResult> CompleteFetchLogSqlServerAsync(
        long logId,
        string status,
        int? ratesImported,
        int? ratesUpdated,
        string? errorMessage,
        CancellationToken cancellationToken)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@LogId", logId);
        parameters.Add("@Status", status);
        parameters.Add("@RatesImported", ratesImported);
        parameters.Add("@RatesUpdated", ratesUpdated);
        parameters.Add("@ErrorMessage", errorMessage);

        var result = await connection.QuerySingleAsync<CompleteFetchLogResult>(
            "[dbo].[sp_CompleteFetchLog]",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    private async Task<CompleteFetchLogResult> CompleteFetchLogSqliteAsync(
        long logId,
        string status,
        int? ratesImported,
        int? ratesUpdated,
        string? errorMessage,
        CancellationToken cancellationToken)
    {
        // Retry logic for SQLite concurrency issues
        const int maxRetries = 5;
        var random = new Random();

        for (int retry = 0; retry <= maxRetries; retry++)
        {
            try
            {
                return await ExecuteCompleteFetchLogWithTransactionAsync(
                    logId, status, ratesImported, ratesUpdated, errorMessage, cancellationToken);
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex) when (
                (ex.SqliteErrorCode == 5 || ex.SqliteErrorCode == 6) && // SQLITE_BUSY or SQLITE_LOCKED
                retry < maxRetries)
            {
                var delayMs = (50 * Math.Pow(2, retry)) + random.Next(0, (int)(50 * Math.Pow(2, retry)));
                await Task.Delay((int)delayMs, cancellationToken);
                continue;
            }
        }

        throw new InvalidOperationException("Complete fetch log failed after multiple retries due to database locking");
    }

    private async Task<CompleteFetchLogResult> ExecuteCompleteFetchLogWithTransactionAsync(
        long logId,
        string status,
        int? ratesImported,
        int? ratesUpdated,
        string? errorMessage,
        CancellationToken cancellationToken)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        try
        {
            // Validate status value
            if (status != "Success" && status != "Failed" && status != "PartialSuccess")
                throw new InvalidOperationException("Invalid status. Must be Success, Failed, or PartialSuccess");

            // Get current log details
            var logInfo = await connection.QuerySingleOrDefaultAsync<(int ProviderId, string Status, string? FetchCompleted)>(
                "SELECT ProviderId, Status, FetchCompleted FROM ExchangeRateFetchLog WHERE Id = @LogId",
                new { LogId = logId },
                transaction);

            if (logInfo.ProviderId == 0)
                throw new InvalidOperationException("Fetch log not found");

            if (logInfo.FetchCompleted != null)
                throw new InvalidOperationException("Fetch log already completed");

            if (logInfo.Status != "Running")
                throw new InvalidOperationException($"Cannot complete fetch log with status {logInfo.Status}. Expected Running.");

            // Update fetch log
            var rowsAffected = await connection.ExecuteAsync(
                @"UPDATE ExchangeRateFetchLog
                  SET FetchCompleted = datetime('now'),
                      Status = @Status,
                      RatesImported = @RatesImported,
                      RatesUpdated = @RatesUpdated,
                      ErrorMessage = @ErrorMessage
                  WHERE Id = @LogId AND FetchCompleted IS NULL",
                new
                {
                    LogId = logId,
                    Status = status,
                    RatesImported = ratesImported,
                    RatesUpdated = ratesUpdated,
                    ErrorMessage = errorMessage
                },
                transaction);

            if (rowsAffected == 0)
                throw new InvalidOperationException("Failed to update fetch log (possible race condition)");

            // Update provider health status based on result
            int? consecutiveFailures = null;

            if (status == "Success")
            {
                await connection.ExecuteAsync(
                    @"UPDATE ExchangeRateProvider
                      SET LastSuccessfulFetch = datetime('now'),
                          ConsecutiveFailures = 0,
                          Modified = datetime('now')
                      WHERE Id = @ProviderId",
                    new { ProviderId = logInfo.ProviderId },
                    transaction);
                consecutiveFailures = 0;
            }
            else if (status == "PartialSuccess")
            {
                await connection.ExecuteAsync(
                    @"UPDATE ExchangeRateProvider
                      SET LastSuccessfulFetch = datetime('now'),
                          Modified = datetime('now')
                      WHERE Id = @ProviderId",
                    new { ProviderId = logInfo.ProviderId },
                    transaction);
            }
            else if (status == "Failed")
            {
                await connection.ExecuteAsync(
                    @"UPDATE ExchangeRateProvider
                      SET LastFailedFetch = datetime('now'),
                          ConsecutiveFailures = ConsecutiveFailures + 1,
                          Modified = datetime('now')
                      WHERE Id = @ProviderId",
                    new { ProviderId = logInfo.ProviderId },
                    transaction);

                consecutiveFailures = await connection.ExecuteScalarAsync<int>(
                    "SELECT ConsecutiveFailures FROM ExchangeRateProvider WHERE Id = @ProviderId",
                    new { ProviderId = logInfo.ProviderId },
                    transaction);

                // Auto-disable after 10 consecutive failures
                if (consecutiveFailures >= 10)
                {
                    await connection.ExecuteAsync(
                        @"UPDATE ExchangeRateProvider
                          SET IsActive = 0,
                              Modified = datetime('now')
                          WHERE Id = @ProviderId",
                        new { ProviderId = logInfo.ProviderId },
                        transaction);

                    await connection.ExecuteAsync(
                        @"INSERT INTO ErrorLog (Timestamp, Severity, Source, Message)
                          VALUES (datetime('now'), 'Critical', 'CompleteFetchLog', @Message)",
                        new { Message = $"Provider {logInfo.ProviderId} automatically disabled after {consecutiveFailures} consecutive failures" },
                        transaction);
                }
            }

            transaction.Commit();

            return new CompleteFetchLogResult
            {
                LogId = logId,
                Status = status,
                RatesImported = ratesImported,
                RatesUpdated = ratesUpdated,
                ProviderId = logInfo.ProviderId,
                ProviderConsecutiveFailures = consecutiveFailures,
                CompletionStatus = "SUCCESS"
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new InvalidOperationException($"Complete fetch log failed: {ex.Message}", ex);
        }
    }

    public async Task<long> LogErrorAsync(
        string severity,
        string source,
        string message,
        string? exception = null,
        string? stackTrace = null,
        CancellationToken cancellationToken = default)
    {
        var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        if (_useInMemoryDatabase)
        {
            return await LogErrorSqliteAsync(connection, severity, source, message, exception, stackTrace, cancellationToken);
        }
        else
        {
            return await LogErrorSqlServerAsync(connection, severity, source, message, exception, stackTrace, cancellationToken);
        }
    }

    private async Task<long> LogErrorSqliteAsync(
        IDbConnection connection,
        string severity,
        string source,
        string message,
        string? exception,
        string? stackTrace,
        CancellationToken cancellationToken)
    {
        var sql = @"
            INSERT INTO ErrorLog (Timestamp, Severity, Source, Message, Exception, StackTrace)
            VALUES (datetime('now'), @Severity, @Source, @Message, @Exception, @StackTrace);
            SELECT last_insert_rowid();";

        return await connection.ExecuteScalarAsync<long>(
            sql,
            new
            {
                Severity = severity,
                Source = source,
                Message = message,
                Exception = exception,
                StackTrace = stackTrace
            });
    }

    private async Task<long> LogErrorSqlServerAsync(
        IDbConnection connection,
        string severity,
        string source,
        string message,
        string? exception,
        string? stackTrace,
        CancellationToken cancellationToken)
    {
        var sql = @"
            INSERT INTO ErrorLog (Timestamp, Severity, Source, Message, Exception, StackTrace)
            VALUES (SYSDATETIMEOFFSET(), @Severity, @Source, @Message, @Exception, @StackTrace);
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

        return await connection.ExecuteScalarAsync<long>(
            sql,
            new
            {
                Severity = severity,
                Source = source,
                Message = message,
                Exception = exception,
                StackTrace = stackTrace
            });
    }
}
