using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryProductGroupRepository : IProductGroupRepository
{
    private readonly ConcurrentDictionary<Guid, ProductGroup> _storage = new();

    public Task AddAsync(ProductGroup entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<ProductGroup>> FindAsync(Expression<Func<ProductGroup, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<ProductGroup>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<ProductGroup?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(ProductGroup entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
