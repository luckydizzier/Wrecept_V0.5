using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface ITaxRateRepository
{
    Task<TaxRate?> GetByIdAsync(Guid id);
    Task<List<TaxRate>> GetAllAsync();
    Task<List<TaxRate>> FindAsync(Expression<Func<TaxRate, bool>> predicate);
    Task AddAsync(TaxRate entity);
    Task UpdateAsync(TaxRate entity);
    Task DeleteAsync(Guid id);
}
