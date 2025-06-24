using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface IInvoiceItemRepository
{
    Task<InvoiceItem?> GetByIdAsync(Guid id);
    Task<List<InvoiceItem>> GetAllAsync();
    Task<List<InvoiceItem>> FindAsync(Expression<Func<InvoiceItem, bool>> predicate);
    Task AddAsync(InvoiceItem entity);
    Task UpdateAsync(InvoiceItem entity);
    Task DeleteAsync(Guid id);
}
