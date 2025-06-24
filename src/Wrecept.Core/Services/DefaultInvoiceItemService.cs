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

    public Task<List<InvoiceItem>> GetAllAsync() => _repository.GetAllAsync();

    public Task<InvoiceItem?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(InvoiceItem entity)
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
