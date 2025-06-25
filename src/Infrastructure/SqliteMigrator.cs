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
        await connection.OpenAsync().ConfigureAwait(false);

        await using var stream = typeof(SqliteMigrator).Assembly
            .GetManifestResourceStream("Wrecept.db.schema_v1.sql")!;
        using var reader = new StreamReader(stream);
        var sql = await reader.ReadToEndAsync().ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
