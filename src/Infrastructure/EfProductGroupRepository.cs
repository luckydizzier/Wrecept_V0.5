using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfProductGroupRepository : IProductGroupRepository
{
    private readonly WreceptDbContext _db;

    public EfProductGroupRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ProductGroup entity)
    {
        _db.ProductGroups.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.ProductGroups.FindAsync(id);
        if (entity != null)
        {
            _db.ProductGroups.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<ProductGroup>> FindAsync(Expression<Func<ProductGroup, bool>> predicate)
    {
        return await _db.ProductGroups.Where(predicate).ToListAsync();
    }

    public async Task<List<ProductGroup>> GetAllAsync()
    {
        return await _db.ProductGroups.ToListAsync();
    }

    public async Task<ProductGroup?> GetByIdAsync(Guid id)
    {
        return await _db.ProductGroups.FindAsync(id);
    }

    public async Task UpdateAsync(ProductGroup entity)
    {
        _db.ProductGroups.Update(entity);
        await _db.SaveChangesAsync();
    }
}
