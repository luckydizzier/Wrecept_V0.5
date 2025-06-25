using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface IUnitRepository
{
    Task<Unit?> GetByIdAsync(Guid id);
    Task<List<Unit>> GetAllAsync();
    Task<List<Unit>> FindAsync(Expression<Func<Unit, bool>> predicate);
    Task AddAsync(Unit entity);
    Task UpdateAsync(Unit entity);
    Task DeleteAsync(Guid id);
}
