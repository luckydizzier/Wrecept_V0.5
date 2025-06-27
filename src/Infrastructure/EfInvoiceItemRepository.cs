using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfInvoiceItemRepository : IInvoiceItemRepository
{
    private readonly WreceptDbContext _db;

    public EfInvoiceItemRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(InvoiceItem entity)
    {
        _db.InvoiceItems.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.InvoiceItems.FindAsync(id);
        if (entity != null)
        {
            _db.InvoiceItems.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<InvoiceItem>> FindAsync(Expression<Func<InvoiceItem, bool>> predicate)
    {
        return await _db.InvoiceItems.Include(ii => ii.Product)
            .Include(ii => ii.Unit)
            .Where(predicate).ToListAsync();
    }

    public async Task<List<InvoiceItem>> GetAllAsync()
    {
        return await _db.InvoiceItems.Include(ii => ii.Product)
            .Include(ii => ii.Unit)
            .ToListAsync();
    }

    public async Task<InvoiceItem?> GetByIdAsync(Guid id)
    {
        return await _db.InvoiceItems.Include(ii => ii.Product)
            .Include(ii => ii.Unit)
            .FirstOrDefaultAsync(ii => ii.Id == id);
    }

    public async Task UpdateAsync(InvoiceItem entity)
    {
        _db.InvoiceItems.Update(entity);
        await _db.SaveChangesAsync();
    }
}
