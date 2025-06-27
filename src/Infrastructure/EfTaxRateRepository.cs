using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfTaxRateRepository : ITaxRateRepository
{
    private readonly WreceptDbContext _db;

    public EfTaxRateRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(TaxRate entity)
    {
        _db.TaxRates.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.TaxRates.FindAsync(id);
        if (entity != null)
        {
            _db.TaxRates.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<TaxRate>> FindAsync(Expression<Func<TaxRate, bool>> predicate)
    {
        return await _db.TaxRates.Where(predicate).ToListAsync();
    }

    public async Task<List<TaxRate>> GetAllAsync()
    {
        return await _db.TaxRates.ToListAsync();
    }

    public async Task<TaxRate?> GetByIdAsync(Guid id)
    {
        return await _db.TaxRates.FindAsync(id);
    }

    public async Task UpdateAsync(TaxRate entity)
    {
        _db.TaxRates.Update(entity);
        await _db.SaveChangesAsync();
    }
}
