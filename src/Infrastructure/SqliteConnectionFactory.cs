using Microsoft.Data.Sqlite;

namespace Wrecept.Infrastructure;

public class SqliteConnectionFactory
{
    public string DbPath { get; }

    public SqliteConnectionFactory(string dbPath)
    {
        DbPath = dbPath;
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={DbPath}");
    }
}
