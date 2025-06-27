using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfSupplierRepository : ISupplierRepository
{
    private readonly WreceptDbContext _db;

    public EfSupplierRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Supplier entity)
    {
        _db.Suppliers.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.Suppliers.FindAsync(id);
        if (entity != null)
        {
            _db.Suppliers.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate)
    {
        return await _db.Suppliers.Where(predicate).ToListAsync();
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _db.Suppliers.ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        return await _db.Suppliers.FindAsync(id);
    }

    public async Task UpdateAsync(Supplier entity)
    {
        _db.Suppliers.Update(entity);
        await _db.SaveChangesAsync();
    }
}
