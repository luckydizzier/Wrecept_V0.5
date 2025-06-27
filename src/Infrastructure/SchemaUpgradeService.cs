using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Wrecept.Infrastructure;

public static class SchemaUpgradeService
{
    public static void EnsureLatestSchema(WreceptDbContext db)
    {
        using var command = db.Database.GetDbConnection().CreateCommand();
        command.CommandText = "PRAGMA table_info(Invoices);";
        db.Database.OpenConnection();
        var hasCalculation = false;
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var name = reader.GetString(1);
                if (string.Equals(name, "CalculationMode", StringComparison.OrdinalIgnoreCase))
                {
                    hasCalculation = true;
                    break;
                }
            }
        }
        if (!hasCalculation)
        {
            using var alter = db.Database.GetDbConnection().CreateCommand();
            alter.CommandText = "ALTER TABLE Invoices ADD COLUMN CalculationMode INTEGER NOT NULL DEFAULT 0;";
            alter.ExecuteNonQuery();
        }
        db.Database.CloseConnection();
    }
}
