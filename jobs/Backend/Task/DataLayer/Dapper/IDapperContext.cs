using System.Data;

namespace DataLayer.Dapper;

public interface IDapperContext
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
