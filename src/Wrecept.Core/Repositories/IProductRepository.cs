using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
    Task AddAsync(Product entity);
    Task UpdateAsync(Product entity);
    Task DeleteAsync(Guid id);
}
