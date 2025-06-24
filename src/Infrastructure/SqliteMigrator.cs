namespace Wrecept.Infrastructure;

using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

public static class SqliteMigrator
{
    public static async Task EnsureCreatedAsync(string dbPath)
    {
        var dir = Path.GetDirectoryName(dbPath) ?? string.Empty;
        Directory.CreateDirectory(dir);
        await using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var schemaPath = Path.Combine(AppContext.BaseDirectory, "db", "schema_v1.sql");
        var sql = await File.ReadAllTextAsync(schemaPath);
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }
}
