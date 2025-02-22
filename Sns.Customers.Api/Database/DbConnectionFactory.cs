using Microsoft.Data.Sqlite;
using System.Data;

namespace Sns.Customers.Api.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}

public class SqliteConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        SqliteConnection connection = new(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
