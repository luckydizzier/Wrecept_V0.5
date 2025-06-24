using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public interface IPaymentMethodRepository
{
    Task<PaymentMethod?> GetByIdAsync(Guid id);
    Task<List<PaymentMethod>> GetAllAsync();
    Task<List<PaymentMethod>> FindAsync(Expression<Func<PaymentMethod, bool>> predicate);
    Task AddAsync(PaymentMethod entity);
    Task UpdateAsync(PaymentMethod entity);
    Task DeleteAsync(Guid id);
}
