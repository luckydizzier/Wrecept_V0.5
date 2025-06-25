using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqliteUnitRepository : IUnitRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteUnitRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(Unit entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO Units (Id, Name, Symbol)
                                VALUES (@Id, @Name, @Symbol);",
            new { Id = entity.Id.ToString(), entity.Name, entity.Symbol });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Units WHERE Id = @id", new { id = id.ToString() });
    }

    public async Task<List<Unit>> FindAsync(Expression<Func<Unit, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<Unit>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, Name, Symbol FROM Units");
        return rows.Select(r => new Unit { Id = Guid.Parse(r.Id.ToString()), Name = r.Name, Symbol = r.Symbol }).ToList();
    }

    public async Task<Unit?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, Name, Symbol FROM Units WHERE Id = @id", new { id = id.ToString() });
        if (row == null) return null;
        return new Unit { Id = Guid.Parse(row.Id.ToString()), Name = row.Name, Symbol = row.Symbol };
    }

    public async Task UpdateAsync(Unit entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("UPDATE Units SET Name=@Name, Symbol=@Symbol WHERE Id=@Id",
            new { Id = entity.Id.ToString(), entity.Name, entity.Symbol });
    }
}
