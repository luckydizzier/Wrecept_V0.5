using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemorySupplierRepository : ISupplierRepository
{
    private readonly ConcurrentDictionary<Guid, Supplier> _storage = new();

    public Task AddAsync(Supplier entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<Supplier>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<Supplier?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Supplier entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
