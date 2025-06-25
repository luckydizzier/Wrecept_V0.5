using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqliteProductGroupRepository : IProductGroupRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteProductGroupRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(ProductGroup entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO ProductGroups (Id, Name)
                                VALUES (@Id, @Name);",
            new { Id = entity.Id.ToString(), entity.Name });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM ProductGroups WHERE Id = @id", new { id = id.ToString() });
    }

    public async Task<List<ProductGroup>> FindAsync(Expression<Func<ProductGroup, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<ProductGroup>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, Name FROM ProductGroups");
        return rows.Select(r => new ProductGroup { Id = Guid.Parse(r.Id.ToString()), Name = r.Name }).ToList();
    }

    public async Task<ProductGroup?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, Name FROM ProductGroups WHERE Id = @id", new { id = id.ToString() });
        if (row == null) return null;
        return new ProductGroup { Id = Guid.Parse(row.Id.ToString()), Name = row.Name };
    }

    public async Task UpdateAsync(ProductGroup entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("UPDATE ProductGroups SET Name=@Name WHERE Id=@Id",
            new { Id = entity.Id.ToString(), entity.Name });
    }
}
