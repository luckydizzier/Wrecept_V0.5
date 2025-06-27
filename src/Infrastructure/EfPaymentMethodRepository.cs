using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

namespace Wrecept.Infrastructure;

public class EfPaymentMethodRepository : IPaymentMethodRepository
{
    private readonly WreceptDbContext _db;

    public EfPaymentMethodRepository(WreceptDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(PaymentMethod entity)
    {
        _db.PaymentMethods.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.PaymentMethods.FindAsync(id);
        if (entity != null)
        {
            _db.PaymentMethods.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<PaymentMethod>> FindAsync(Expression<Func<PaymentMethod, bool>> predicate)
    {
        return await _db.PaymentMethods.Where(predicate).ToListAsync();
    }

    public async Task<List<PaymentMethod>> GetAllAsync()
    {
        return await _db.PaymentMethods.ToListAsync();
    }

    public async Task<PaymentMethod?> GetByIdAsync(Guid id)
    {
        return await _db.PaymentMethods.FindAsync(id);
    }

    public async Task UpdateAsync(PaymentMethod entity)
    {
        _db.PaymentMethods.Update(entity);
        await _db.SaveChangesAsync();
    }
}
