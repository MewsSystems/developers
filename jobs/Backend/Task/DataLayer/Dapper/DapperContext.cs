using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataLayer.Dapper;

public class DapperContext : IDapperContext
{
    private readonly string _connectionString;
    private readonly bool _useInMemoryDatabase;

    public DapperContext(IConfiguration configuration)
    {
        _useInMemoryDatabase = configuration.GetValue<bool>("Database:UseInMemoryDatabase");

        if (_useInMemoryDatabase)
        {
            // Use shared cache to ensure all connections see the same in-memory database
            _connectionString = "DataSource=file:memdb1?mode=memory&cache=shared";
        }
        else
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
    }

    public IDbConnection CreateConnection()
    {
        if (_useInMemoryDatabase)
        {
            return new SqliteConnection(_connectionString);
        }
        return new SqlConnection(_connectionString);
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_useInMemoryDatabase)
        {
            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            // Configure SQLite for better concurrency
            using (var command = connection.CreateCommand())
            {
                // Enable WAL mode for better concurrent read/write performance
                command.CommandText = "PRAGMA journal_mode = WAL;";
                await command.ExecuteNonQueryAsync(cancellationToken);

                // Set busy timeout to 30 seconds (waits instead of immediately failing on lock)
                command.CommandText = "PRAGMA busy_timeout = 30000;";
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return connection;
        }
        else
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
