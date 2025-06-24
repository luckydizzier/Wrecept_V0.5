using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _storage = new();

    public Task AddAsync(Product entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<Product>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Product entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
