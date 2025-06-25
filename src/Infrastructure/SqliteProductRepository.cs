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
                Id = entity.Id.ToString(),
                entity.Name,
                GroupId = entity.Group.Id.ToString(),
                TaxRateId = entity.TaxRate.Id.ToString(),
                DefaultUnitId = entity.DefaultUnit.Id.ToString()
            });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Products WHERE Id = @id", new { id = id.ToString() });
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
        return rows.Select(r => new Product
        {
            Id = Guid.Parse(r.Id.ToString()),
            Name = r.Name,
            Group = new ProductGroup(),
            TaxRate = new TaxRate(),
            DefaultUnit = new Unit()
        }).ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, Name FROM Products WHERE Id = @id", new { id = id.ToString() });
        if (row == null) return null;
        return new Product { Id = Guid.Parse(row.Id.ToString()), Name = row.Name, Group = new ProductGroup(), TaxRate = new TaxRate(), DefaultUnit = new Unit() };
    }

    public async Task UpdateAsync(Product entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"UPDATE Products SET Name=@Name WHERE Id=@Id",
            new { Id = entity.Id.ToString(), entity.Name });
    }
}
