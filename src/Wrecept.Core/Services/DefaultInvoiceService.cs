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

    public Task<List<Invoice>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load invoices.");

    public Task<List<Invoice>> GetByDateRange(DateOnly? from, DateOnly? to)
    {
        return ServiceUtil.WrapAsync(() => _repository.FindAsync(i =>
                (!from.HasValue || i.IssueDate >= from.Value) &&
                (!to.HasValue || i.IssueDate <= to.Value)),
            "Failed to filter invoices.");
    }

    public Task<List<Invoice>> GetBySupplierId(Guid supplierId) =>
        ServiceUtil.WrapAsync(() => _repository.GetBySupplierIdAsync(supplierId),
            "Failed to query invoices by supplier.");

    public Task<List<Invoice>> GetByProductGroupId(Guid groupId) =>
        ServiceUtil.WrapAsync(() => _repository.GetByProductGroupIdAsync(groupId),
            "Failed to query invoices by product group.");

    public Task<List<Invoice>> GetByProductId(Guid productId) =>
        ServiceUtil.WrapAsync(() => _repository.GetByProductIdAsync(productId),
            "Failed to query invoices by product.");

    public Task<Invoice?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id),
            "Failed to load invoice.");

    public async Task SaveAsync(Invoice entity)
    {
        await ServiceUtil.WrapAsync(async () =>
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
        }, "Failed to save invoice.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete invoice.");
}
