using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfProductRepository : IProductRepository
{
    private readonly WreceptDbContext _db;

    public EfProductRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Product entity)
    {
        _db.Products.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity != null)
        {
            _db.Products.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _db.Products.Include(p => p.Group)
            .Include(p => p.TaxRate)
            .Include(p => p.DefaultUnit)
            .Where(predicate).ToListAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products.Include(p => p.Group)
            .Include(p => p.TaxRate)
            .Include(p => p.DefaultUnit)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _db.Products.Include(p => p.Group)
            .Include(p => p.TaxRate)
            .Include(p => p.DefaultUnit)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Product entity)
    {
        _db.Products.Update(entity);
        await _db.SaveChangesAsync();
    }
}
