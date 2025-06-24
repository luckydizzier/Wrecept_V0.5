using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryInvoiceItemRepository : IInvoiceItemRepository
{
    private readonly ConcurrentDictionary<Guid, InvoiceItem> _storage = new();

    public Task AddAsync(InvoiceItem entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<InvoiceItem>> FindAsync(Expression<Func<InvoiceItem, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<InvoiceItem>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<InvoiceItem?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(InvoiceItem entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
