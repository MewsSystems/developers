using Dapper;
using DataLayer.DTOs;
using System.Data;
using System.Text.Json;

namespace DataLayer.Dapper;

public class StoredProcedureService : IStoredProcedureService
{
    private readonly IDapperContext _dapperContext;

    public StoredProcedureService(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<BulkUpsertResult> BulkUpsertExchangeRatesAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<ExchangeRateInput> rates,
        CancellationToken cancellationToken = default)
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

    public async Task<StartFetchLogResult> StartFetchLogAsync(
        int providerId,
        int? requestedBy = null,
        CancellationToken cancellationToken = default)
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

    public async Task<CompleteFetchLogResult> CompleteFetchLogAsync(
        long logId,
        string status,
        int? ratesImported = null,
        int? ratesUpdated = null,
        string? errorMessage = null,
        CancellationToken cancellationToken = default)
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
}
