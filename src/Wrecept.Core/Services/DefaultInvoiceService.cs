namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface IInvoiceService
{
    Task<List<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<List<Invoice>> GetByDateRange(DateOnly? from, DateOnly? to);
    Task<List<Invoice>> GetBySupplierId(Guid supplierId);
    Task<List<Invoice>> GetByProductGroupId(Guid groupId);
    Task<List<Invoice>> GetByProductId(Guid productId);
    Task SaveAsync(Invoice entity);
    Task DeleteAsync(Guid id);
}

public class DefaultInvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;

    public DefaultInvoiceService(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Invoice>> GetAllAsync() => _repository.GetAllAsync();

    public Task<List<Invoice>> GetByDateRange(DateOnly? from, DateOnly? to)
    {
        return _repository.FindAsync(i =>
            (!from.HasValue || i.IssueDate >= from.Value) &&
            (!to.HasValue || i.IssueDate <= to.Value));
    }

    public Task<List<Invoice>> GetBySupplierId(Guid supplierId) =>
        _repository.GetBySupplierIdAsync(supplierId);

    public Task<List<Invoice>> GetByProductGroupId(Guid groupId) =>
        _repository.GetByProductGroupIdAsync(groupId);

    public Task<List<Invoice>> GetByProductId(Guid productId) =>
        _repository.GetByProductIdAsync(productId);

    public Task<Invoice?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(Invoice entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
            await _repository.AddAsync(entity);
        }
        else
        {
            await _repository.UpdateAsync(entity);
        }
    }

    public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
}
