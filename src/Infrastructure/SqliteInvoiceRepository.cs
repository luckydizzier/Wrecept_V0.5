using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class SqliteInvoiceRepository : IInvoiceRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteInvoiceRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(Invoice entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"INSERT INTO Invoices (Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes)
                                VALUES (@Id, @SerialNumber, @IssueDate, @SupplierId, @PaymentMethodId, @Notes);",
            new
            {
                entity.Id,
                entity.SerialNumber,
                IssueDate = entity.IssueDate.ToString("yyyy-MM-dd"),
                SupplierId = entity.Supplier.Id,
                PaymentMethodId = entity.PaymentMethod.Id,
                entity.Notes
            });
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Invoices WHERE Id = @id", new { id });
    }

    public async Task<List<Invoice>> FindAsync(Expression<Func<Invoice, bool>> predicate)
    {
        var all = await GetAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }

    public async Task<List<Invoice>> GetBySupplierIdAsync(Guid supplierId)
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes FROM Invoices WHERE SupplierId = @sid", new { sid = supplierId });
        return rows.Select(r => new Invoice
        {
            Id = Guid.Parse(r.Id.ToString()),
            SerialNumber = r.SerialNumber,
            IssueDate = DateOnly.Parse(r.IssueDate),
            Supplier = new Supplier { Id = Guid.Parse(r.SupplierId.ToString()), Name = string.Empty },
            PaymentMethod = new PaymentMethod { Id = Guid.Parse(r.PaymentMethodId.ToString()), Label = string.Empty },
            Notes = r.Notes ?? string.Empty
        }).ToList();
    }

    public async Task<List<Invoice>> GetByProductGroupIdAsync(Guid groupId)
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync(@"SELECT i.Id, i.SerialNumber, i.IssueDate, i.SupplierId, i.PaymentMethodId, i.Notes
                                           FROM Invoices i
                                           JOIN InvoiceItems it ON i.Id = it.InvoiceId
                                           JOIN Products p ON it.ProductId = p.Id
                                           WHERE p.ProductGroupId = @gid", new { gid = groupId });
        return rows.Select(r => new Invoice
        {
            Id = Guid.Parse(r.Id.ToString()),
            SerialNumber = r.SerialNumber,
            IssueDate = DateOnly.Parse(r.IssueDate),
            Supplier = new Supplier { Id = Guid.Parse(r.SupplierId.ToString()), Name = string.Empty },
            PaymentMethod = new PaymentMethod { Id = Guid.Parse(r.PaymentMethodId.ToString()), Label = string.Empty },
            Notes = r.Notes ?? string.Empty
        }).Distinct().ToList();
    }

    public async Task<List<Invoice>> GetByProductIdAsync(Guid productId)
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync(@"SELECT i.Id, i.SerialNumber, i.IssueDate, i.SupplierId, i.PaymentMethodId, i.Notes
                                           FROM Invoices i
                                           JOIN InvoiceItems it ON i.Id = it.InvoiceId
                                           WHERE it.ProductId = @pid", new { pid = productId });
        return rows.Select(r => new Invoice
        {
            Id = Guid.Parse(r.Id.ToString()),
            SerialNumber = r.SerialNumber,
            IssueDate = DateOnly.Parse(r.IssueDate),
            Supplier = new Supplier { Id = Guid.Parse(r.SupplierId.ToString()), Name = string.Empty },
            PaymentMethod = new PaymentMethod { Id = Guid.Parse(r.PaymentMethodId.ToString()), Label = string.Empty },
            Notes = r.Notes ?? string.Empty
        }).Distinct().ToList();
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        await using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync("SELECT Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes FROM Invoices");
        return rows.Select(r => new Invoice
        {
            Id = Guid.Parse(r.Id.ToString()),
            SerialNumber = r.SerialNumber,
            IssueDate = DateOnly.Parse(r.IssueDate),
            Supplier = new Supplier { Id = Guid.Parse(r.SupplierId.ToString()), Name = string.Empty },
            PaymentMethod = new PaymentMethod { Id = Guid.Parse(r.PaymentMethodId.ToString()), Label = string.Empty },
            Notes = r.Notes ?? string.Empty
        }).ToList();
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        await using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync("SELECT Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes FROM Invoices WHERE Id = @id", new { id });
        if (row == null) return null;
        return new Invoice
        {
            Id = Guid.Parse(row.Id.ToString()),
            SerialNumber = row.SerialNumber,
            IssueDate = DateOnly.Parse(row.IssueDate),
            Supplier = new Supplier { Id = Guid.Parse(row.SupplierId.ToString()), Name = string.Empty },
            PaymentMethod = new PaymentMethod { Id = Guid.Parse(row.PaymentMethodId.ToString()), Label = string.Empty },
            Notes = row.Notes ?? string.Empty
        };
    }

    public async Task UpdateAsync(Invoice entity)
    {
        await using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"UPDATE Invoices SET SerialNumber=@SerialNumber, IssueDate=@IssueDate, SupplierId=@SupplierId, PaymentMethodId=@PaymentMethodId, Notes=@Notes WHERE Id=@Id",
            new
            {
                entity.Id,
                entity.SerialNumber,
                IssueDate = entity.IssueDate.ToString("yyyy-MM-dd"),
                SupplierId = entity.Supplier.Id,
                PaymentMethodId = entity.PaymentMethod.Id,
                entity.Notes
            });
    }
}
