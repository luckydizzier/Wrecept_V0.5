using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqliteSupplierRepository : ISupplierRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteSupplierRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(Supplier entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO Suppliers (Id, Name, Address, TaxId, BankAccountNumber)
                                VALUES (@Id, @Name, @Address, @TaxId, @BankAccountNumber);",
            new
            {
                Id = entity.Id.ToString(),
                entity.Name,
                entity.Address,
                entity.TaxId,
                entity.BankAccountNumber
            });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Suppliers WHERE Id = @id", new { id = id.ToString() });
    }

    public async Task<List<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var result = await conn.QueryAsync<Supplier>("SELECT Id, Name, Address, TaxId, BankAccountNumber FROM Suppliers");
        return result.ToList();
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Supplier>("SELECT Id, Name, Address, TaxId, BankAccountNumber FROM Suppliers WHERE Id = @id", new { id = id.ToString() });
    }

    public async Task UpdateAsync(Supplier entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"UPDATE Suppliers SET Name=@Name, Address=@Address, TaxId=@TaxId, BankAccountNumber=@BankAccountNumber WHERE Id=@Id",
            new
            {
                Id = entity.Id.ToString(),
                entity.Name,
                entity.Address,
                entity.TaxId,
                entity.BankAccountNumber
            });
    }
}
