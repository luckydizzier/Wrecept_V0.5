using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryUnitRepository : IUnitRepository
{
    private readonly ConcurrentDictionary<Guid, Unit> _storage = new();

    public Task AddAsync(Unit entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<Unit>> FindAsync(Expression<Func<Unit, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<Unit>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<Unit?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Unit entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
