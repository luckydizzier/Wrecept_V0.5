using System.Collections.Concurrent;
using System.Linq.Expressions;
using Wrecept.Core.Domain;

namespace Wrecept.Core.Repositories;

public class InMemoryInvoiceRepository : IInvoiceRepository
{
    private readonly ConcurrentDictionary<Guid, Invoice> _storage = new();

    public InMemoryInvoiceRepository()
    {
    }

    public InMemoryInvoiceRepository(IEnumerable<Invoice> seed)
    {
        foreach (var invoice in seed)
        {
            _storage[invoice.Id] = invoice;
        }
    }

    public Task AddAsync(Invoice entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<List<Invoice>> FindAsync(Expression<Func<Invoice, bool>> predicate)
    {
        var query = _storage.Values.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(query);
    }

    public Task<List<Invoice>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.ToList());
    }

    public Task<List<Invoice>> GetBySupplierIdAsync(Guid supplierId)
    {
        var result = _storage.Values.Where(i => i.Supplier.Id == supplierId).ToList();
        return Task.FromResult(result);
    }

    public Task<List<Invoice>> GetByProductGroupIdAsync(Guid groupId)
    {
        var result = _storage.Values.Where(i => i.Items.Any(it => it.Product.Group.Id == groupId)).ToList();
        return Task.FromResult(result);
    }

    public Task<List<Invoice>> GetByProductIdAsync(Guid productId)
    {
        var result = _storage.Values.Where(i => i.Items.Any(it => it.Product.Id == productId)).ToList();
        return Task.FromResult(result);
    }

    public Task<Invoice?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Invoice entity)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
