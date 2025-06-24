using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryPaymentMethodRepository : IPaymentMethodRepository
{
    private readonly ConcurrentDictionary<Guid, PaymentMethod> _storage = new();

    public Task AddAsync(PaymentMethod entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<PaymentMethod>> FindAsync(Expression<Func<PaymentMethod, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<PaymentMethod>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<PaymentMethod?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(PaymentMethod entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
