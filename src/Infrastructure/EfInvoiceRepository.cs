using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfInvoiceRepository : IInvoiceRepository
{
    private readonly WreceptDbContext _db;

    public EfInvoiceRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Invoice entity)
    {
        _db.Invoices.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.Invoices.FindAsync(id);
        if (entity != null)
        {
            _db.Invoices.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Invoice>> FindAsync(Expression<Func<Invoice, bool>> predicate)
    {
        return await _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items).ThenInclude(it => it.Product)
            .ThenInclude(p => p.Group)
            .Where(predicate).ToListAsync();
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        return await _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items).ThenInclude(it => it.Product)
            .ThenInclude(p => p.Group)
            .ToListAsync();
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items).ThenInclude(it => it.Product)
            .ThenInclude(p => p.Group)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Invoice>> GetBySupplierIdAsync(Guid supplierId)
    {
        return await _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Where(i => EF.Property<Guid>(i, "SupplierId") == supplierId)
            .ToListAsync();
    }

    public async Task<List<Invoice>> GetByProductGroupIdAsync(Guid groupId)
    {
        return await _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items).ThenInclude(it => it.Product)
            .Where(i => i.Items.Any(it => EF.Property<Guid>(it.Product, "ProductGroupId") == groupId))
            .ToListAsync();
    }

    public async Task<List<Invoice>> GetByProductIdAsync(Guid productId)
    {
        return await _db.Invoices
            .Include(i => i.Supplier)
            .Include(i => i.PaymentMethod)
            .Include(i => i.Items)
            .Where(i => i.Items.Any(it => EF.Property<Guid>(it.Product, "Id") == productId))
            .ToListAsync();
    }

    public async Task UpdateAsync(Invoice entity)
    {
        _db.Invoices.Update(entity);
        await _db.SaveChangesAsync();
    }
}
