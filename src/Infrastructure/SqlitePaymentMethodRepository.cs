using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqlitePaymentMethodRepository : IPaymentMethodRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqlitePaymentMethodRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(PaymentMethod entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO PaymentMethods (Id, Label)
                                VALUES (@Id, @Label);",
            new { Id = entity.Id.ToString(), entity.Label });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM PaymentMethods WHERE Id = @id", new { id = id.ToString() });
    }

    public async Task<List<PaymentMethod>> FindAsync(Expression<Func<PaymentMethod, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<PaymentMethod>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, Label FROM PaymentMethods");
        return rows.Select(r => new PaymentMethod { Id = Guid.Parse(r.Id.ToString()), Label = r.Label }).ToList();
    }

    public async Task<PaymentMethod?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, Label FROM PaymentMethods WHERE Id = @id", new { id = id.ToString() });
        if (row == null) return null;
        return new PaymentMethod { Id = Guid.Parse(row.Id.ToString()), Label = row.Label };
    }

    public async Task UpdateAsync(PaymentMethod entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("UPDATE PaymentMethods SET Label=@Label WHERE Id=@Id",
            new { Id = entity.Id.ToString(), entity.Label });
    }
}
