using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqliteProductRepository : IProductRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteProductRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(Product entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO Products (Id, Name, ProductGroupId, TaxRateId, DefaultUnitId)
                                VALUES (@Id, @Name, @GroupId, @TaxRateId, @DefaultUnitId);",
            new
            {
                entity.Id,
                entity.Name,
                GroupId = entity.Group.Id,
                TaxRateId = entity.TaxRate.Id,
                DefaultUnitId = entity.DefaultUnit.Id
            });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Products WHERE Id = @id", new { id });
    }

    public async Task<List<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, Name FROM Products");
        return rows.Select(r => new Product { Id = r.Id, Name = r.Name, Group = new ProductGroup(), TaxRate = new TaxRate(), DefaultUnit = new Unit() }).ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, Name FROM Products WHERE Id = @id", new { id });
        if (row == null) return null;
        return new Product { Id = row.Id, Name = row.Name, Group = new ProductGroup(), TaxRate = new TaxRate(), DefaultUnit = new Unit() };
    }

    public async Task UpdateAsync(Product entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"UPDATE Products SET Name=@Name WHERE Id=@Id", new { entity.Id, entity.Name });
    }
}
