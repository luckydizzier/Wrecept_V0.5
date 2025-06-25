using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqliteTaxRateRepository : ITaxRateRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteTaxRateRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(TaxRate entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO TaxRates (Id, Label, Percentage)
                                VALUES (@Id, @Label, @Percentage);",
            new { Id = entity.Id.ToString(), entity.Label, entity.Percentage });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM TaxRates WHERE Id = @id", new { id = id.ToString() });
    }

    public async Task<List<TaxRate>> FindAsync(Expression<Func<TaxRate, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<TaxRate>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, Label, Percentage FROM TaxRates");
        return rows.Select(r => new TaxRate { Id = Guid.Parse(r.Id.ToString()), Label = r.Label, Percentage = r.Percentage }).ToList();
    }

    public async Task<TaxRate?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, Label, Percentage FROM TaxRates WHERE Id = @id", new { id = id.ToString() });
        if (row == null) return null;
        return new TaxRate { Id = Guid.Parse(row.Id.ToString()), Label = row.Label, Percentage = row.Percentage };
    }

    public async Task UpdateAsync(TaxRate entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("UPDATE TaxRates SET Label=@Label, Percentage=@Percentage WHERE Id=@Id",
            new { Id = entity.Id.ToString(), entity.Label, entity.Percentage });
    }
}
