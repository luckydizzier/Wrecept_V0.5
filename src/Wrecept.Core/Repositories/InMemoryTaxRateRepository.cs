using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryTaxRateRepository : ITaxRateRepository
{
    private readonly ConcurrentDictionary<Guid, TaxRate> _storage = new();

    public Task AddAsync(TaxRate entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<TaxRate>> FindAsync(Expression<Func<TaxRate, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<TaxRate>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<TaxRate?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(TaxRate entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
