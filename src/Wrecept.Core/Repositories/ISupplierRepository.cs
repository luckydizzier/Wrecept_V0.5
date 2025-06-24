using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id);
    Task<List<Supplier>> GetAllAsync();
    Task<List<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate);
    Task AddAsync(Supplier entity);
    Task UpdateAsync(Supplier entity);
    Task DeleteAsync(Guid id);
}
