namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface IInvoiceItemService
{
    Task<List<InvoiceItem>> GetAllAsync();
    Task<InvoiceItem?> GetByIdAsync(Guid id);
    Task SaveAsync(InvoiceItem entity);
    Task DeleteAsync(Guid id);
}

public class DefaultInvoiceItemService : IInvoiceItemService
{
    private readonly IInvoiceItemRepository _repository;

    public DefaultInvoiceItemService(IInvoiceItemRepository repository)
    {
        _repository = repository;
    }

    public Task<List<InvoiceItem>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load invoice items.");

    public Task<InvoiceItem?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load invoice item.");

    public async Task SaveAsync(InvoiceItem entity)
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
        }, "Failed to save invoice item.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete invoice item.");
}
