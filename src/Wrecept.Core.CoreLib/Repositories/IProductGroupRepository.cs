using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface IProductGroupRepository
{
    Task<ProductGroup?> GetByIdAsync(Guid id);
    Task<List<ProductGroup>> GetAllAsync();
    Task<List<ProductGroup>> FindAsync(Expression<Func<ProductGroup, bool>> predicate);
    Task AddAsync(ProductGroup entity);
    Task UpdateAsync(ProductGroup entity);
    Task DeleteAsync(Guid id);
}
