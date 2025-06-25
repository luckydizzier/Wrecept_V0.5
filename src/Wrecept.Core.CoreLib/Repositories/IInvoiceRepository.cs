using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<List<Invoice>> GetAllAsync();
    Task<List<Invoice>> FindAsync(Expression<Func<Invoice, bool>> predicate);
    Task<List<Invoice>> GetBySupplierIdAsync(Guid supplierId);
    Task<List<Invoice>> GetByProductGroupIdAsync(Guid groupId);
    Task<List<Invoice>> GetByProductIdAsync(Guid productId);
    Task AddAsync(Invoice entity);
    Task UpdateAsync(Invoice entity);
    Task DeleteAsync(Guid id);
}
