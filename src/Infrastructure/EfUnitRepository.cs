using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfUnitRepository : IUnitRepository
{
    private readonly WreceptDbContext _db;

    public EfUnitRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Unit entity)
    {
        _db.Units.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.Units.FindAsync(id);
        if (entity != null)
        {
            _db.Units.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Unit>> FindAsync(Expression<Func<Unit, bool>> predicate)
    {
        return await _db.Units.Where(predicate).ToListAsync();
    }

    public async Task<List<Unit>> GetAllAsync()
    {
        return await _db.Units.ToListAsync();
    }

    public async Task<Unit?> GetByIdAsync(Guid id)
    {
        return await _db.Units.FindAsync(id);
    }

    public async Task UpdateAsync(Unit entity)
    {
        _db.Units.Update(entity);
        await _db.SaveChangesAsync();
    }
}
